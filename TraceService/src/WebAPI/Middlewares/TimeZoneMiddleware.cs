using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TraceService.Application.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Converters;
using System.Globalization;
using TraceService.Application.Convertors;
using TimeZoneConverter;

namespace TraceService.WebAPI.Middlewares
{
    //https://lennilobel.wordpress.com/2021/07/07/implementing-time-zone-support-in-angular-and-net-applications/
    public class TimeZoneMiddleware
    {
        private readonly IConfiguration _config;
        private readonly RequestDelegate _next;
        private readonly CustomDateTimeConverter _isoDateTimeConverter;

        public TimeZoneMiddleware(
            IConfiguration config,
            RequestDelegate next)
        {
            this._config = config;
            this._next = next;
            this._isoDateTimeConverter = new CustomDateTimeConverter();
        }

        public async Task Invoke(HttpContext context)
        {
            // Request content and parameters won't be modified if disableTimeZoneConversion=true is specified
            // as a query string parameter in the URI
            bool disableConversion =
              context.Request.Query.ContainsKey("disableTimeZoneConversion") &&
              context.Request.Query["disableTimeZoneConversion"] == "true";

            // Get the local time zone for UTC conversion
            TimeZoneInfo localTimeZone = this.GetLocalTimeZone(context);

            // If conversion isn't disabled, and the local time zone can be detected (and isn't UTC),
            // modify the request content (convert local to UTC)
            if (!disableConversion && localTimeZone != null && localTimeZone.Id != "UTC")
            {
                // Modify the date/time request parameters in the URI
                ModifyRequestParameters(context, localTimeZone);

                // Don't modify the request content unless the Content-Type is application/json
                bool isJsonContent =
                  context.Request.Headers.ContainsKey("Content-Type") &&
                  context.Request.Headers["Content-Type"] == "application/json";

                if (isJsonContent)
                {
                    // Modify the date/time properties in the request content
                    await ModifyRequestContent(context, localTimeZone);
                }
            }
            else
            {
                ModifyRequestParameters(context, TimeZoneInfo.Utc);
            }
                

            // Prepare for modifying the response body
            Stream responseStream = context.Response.Body;
            MemoryStream modifiedResponseStream = new();
            context.Response.Body = modifiedResponseStream;

            try
            {
                await this._next(context).ConfigureAwait(false);
            }
            finally
            {
                context.Response.Body = responseStream;
            }

            // Modify the response content (convert UTC to local)
            modifiedResponseStream = ModifyResponseContent(context, disableConversion, localTimeZone, modifiedResponseStream);
            await modifiedResponseStream.CopyToAsync(responseStream).ConfigureAwait(false);
        }

        private TimeZoneInfo GetLocalTimeZone(HttpContext context)
        {
            // If the app config doesn't permit multiple time zones, then treat every user as if
            // they were in the same "site" time zone
            if (this._config.GetValue<bool>("TimeZones:SupportMultipleTimeZones") == false)
            {
                return TimeZoneInfo.Utc;
            }
             

            // If the request headers include the user's local time zone (IANA name, injected by client-side HTTP interceptor),
            // use that time zone
            if (context.Request.Headers.TryGetValue("LocalTimeZoneIana", out StringValues localTimeZoneIana))
            {
                return TZConvert.GetTimeZoneInfo(localTimeZoneIana);
            }

            // The app config permits multiple time zones, but the user request doesn't specify the time zone
            return null;
        }

        private static void AdjustUnpsecifiedDateTimeRequestParameters(HttpContext context)
        {
            List<KeyValuePair<string, string>> queryParameters = context.Request.Query.SelectMany(kvp =>kvp.Value, (col, value) => new KeyValuePair<string, string>(col.Key, value)).ToList();

            // Nothing to do if there aren't any
            if (queryParameters.Count == 0)
            {
                return;
            }

            // Build a new list of query parameters, converting date/time values
            List<KeyValuePair<string, string>> modifiedQueryParameters = new();

            bool modified = false;
            foreach (KeyValuePair<string, string> item in queryParameters)
            {
                string value = item.Value;
                if (value.FromDateTimeIsoString(out DateTime dt))
                {
                    DateTime utc = dt.AdjustDateTimeStyle(DateTimeStyles.AssumeUniversal); ;
                    value = utc.ToDateTimeIsoString();
                    KeyValuePair<string, string> modifiedQueryParameter = new(item.Key, value);
                    modifiedQueryParameters.Add(modifiedQueryParameter);
                    modified = true;
                }
                else
                {
                    KeyValuePair<string, string> unmodifiedQueryParameter = new(item.Key, value);
                    modifiedQueryParameters.Add(unmodifiedQueryParameter);
                }
            }

            if (modified)
            {
                QueryBuilder qb = new(modifiedQueryParameters);
                context.Request.QueryString = qb.ToQueryString();
            }
        }

        private static void ModifyRequestParameters(HttpContext context, TimeZoneInfo localTimeZone)
        {
            // Get all the query parameters from the URI
            List<KeyValuePair<string, string>> queryParameters = context.Request.Query
              .SelectMany(kvp =>
                kvp.Value, (col, value) =>
                  new KeyValuePair<string, string>(col.Key, value))
              .ToList();

            // Nothing to do if there aren't any
            if (queryParameters.Count == 0)
            {
                return;
            }

            // Build a new list of query parameters, converting date/time values
            List<KeyValuePair<string, string>> modifiedQueryParameters = new();

            bool modified = false;
            foreach (KeyValuePair<string, string> item in queryParameters)
            {
                string value = item.Value;
                if (value.FromDateTimeIsoString(out DateTime local))
                {
                    DateTime utc = TimeZoneInfo.ConvertTimeToUtc(local, localTimeZone);
                    value = utc.ToDateTimeIsoString();
                    KeyValuePair<string, string> modifiedQueryParameter = new(item.Key, value);
                    modifiedQueryParameters.Add(modifiedQueryParameter);
                    modified = true;
                }
                else
                {
                    KeyValuePair<string, string> unmodifiedQueryParameter = new(item.Key, value);
                    modifiedQueryParameters.Add(unmodifiedQueryParameter);
                }
            }

            if (modified)
            {
                QueryBuilder qb = new(modifiedQueryParameters);
                context.Request.QueryString = qb.ToQueryString();
            }
        }


        private static async Task<TimeZoneInfo> ModifyRequestContent(HttpContext context, TimeZoneInfo localTimeZone)
        {
            // Read the request content from the request body stream; if it's a JSON object, we'll process it
            System.IO.Stream requestStream = context.Request.Body;
            string originalRequestContent = await new StreamReader(requestStream).ReadToEndAsync();

            // Try to get the JSON object from the request content
            JToken jobj = originalRequestContent.TryDeserializeToJToken();

            // If the request content is a JSON object, convert all of it's date/time properties from local time to UTC
            bool modified = false;
            if (jobj != null)
            {
                modified = jobj.ConvertLocalToUtc(localTimeZone);
            }

            if (modified)
            {
                // Replace the stream with the updated request content
                string json = JsonConvert.SerializeObject(jobj,
                                    new StringEnumConverter(),
                                    new CustomDateTimeConverter());
                StringContent requestContent = new(json, Encoding.UTF8, "application/json");
                requestStream = await requestContent.ReadAsStreamAsync();
            }
            else
            {
                // Replace the stream with the original request content
                requestStream = new MemoryStream(Encoding.UTF8.GetBytes(originalRequestContent));
            }

            // Replace the request body stream
            context.Request.Body = requestStream;

            // Return the time zone info for the reverse conversion on the response
            return localTimeZone;
        }
       

        private static MemoryStream ModifyResponseContent(
            HttpContext context,
            bool disableConversion,
            TimeZoneInfo localTimeZone,
            MemoryStream responseStream)
        {
            // Rewind the unmodified response stream
            responseStream.Position = 0;
            bool modified = false;

            // Will capture the unmodified response for time zone conversion
            string responseContent = default;

            // Only attempt to modify the response if time zone conversion is not disabled
            // and we have a local time zone that was used to modify the request
            if (!disableConversion && localTimeZone != null)
            {
                // Capture the unmodified response
                responseContent = new StreamReader(responseStream).ReadToEnd();

                // Try to get the JSON object from the response content
                JToken jobj = responseContent.TryDeserializeToJToken();

                // If the response content is a JSON object, convert all of it's date/time properties from local time to UTC
                if (jobj != null && jobj.ConvertUtcToLocal(localTimeZone))
                {
                    responseContent = JsonConvert.SerializeObject(jobj,
                        new StringEnumConverter(),
                        new CustomDateTimeConverter());
                    modified = true;
                }
            }
            

            // If no changes were made (i.e., there were no converted date/time properties),
            // use the original unmodified response
            if (!modified)
            {
                responseStream.Position = 0;
                context.Response.ContentLength = responseStream.Length;
                return responseStream;
            }

            // Write the changed response content to a new modified response stream
            MemoryStream modifiedResponseStream = new();
            StreamWriter sw = new(modifiedResponseStream);
            sw.Write(responseContent);
            sw.Flush();
            modifiedResponseStream.Position = 0;

            // Use the new modified response
            context.Response.ContentLength = modifiedResponseStream.Length;
            return modifiedResponseStream;
        }
    }
}
