using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using TraceService.Application.Convertors;

namespace TraceService.Application.Extensions
{
    public static class JTokenExtensions
    {
        public static bool ConvertLocalToUtc(this JToken token, TimeZoneInfo localTimeZone, bool wasModified = false)
        {
            bool modified = wasModified;
            if (token.Type == JTokenType.Object)
            {
                modified = ConvertLocalToUtcForObject(token, localTimeZone, wasModified, modified);
            }
            else if (token.Type == JTokenType.Array)
            {
                modified = ConvertLocalToUtcForArray(token, localTimeZone, wasModified, modified);
            }
            return modified;
        }

        public static bool ConvertUtcToLocal(this JToken token, TimeZoneInfo localTimeZone, bool wasModified = false)
        {
            bool modified = wasModified;
            if (token.Type == JTokenType.Object)
            {
                modified = ConvertUtcToLocalForObject(token, localTimeZone, wasModified, modified);
            }
            else if (token.Type == JTokenType.Array)
            {
                modified = ConvertUtcToLocalForArray(token, localTimeZone, wasModified, modified);
            }
            return modified;
        }

        private static bool ConvertLocalToUtcForObject(JToken token, TimeZoneInfo localTimeZone, bool wasModified, bool modified)
        {
            foreach (JProperty prop in token.Children<JProperty>())
            {
                JToken child = prop.Value;
                if (child is JValue jValue)
                {
                    object value = ParseJsonValueForDateTime(jValue.Value);
                    if (value is DateTime local)
                    {
                        DateTime utc = TimeZoneInfo.ConvertTimeToUtc(local, localTimeZone);
                        jValue.Value = utc;
                        modified = true;
                    }
                }
                else if (child.HasValues)
                {
                    modified = child.ConvertLocalToUtc(localTimeZone, wasModified) || modified;
                }
            }

            return modified;
        }

        private static bool ConvertLocalToUtcForArray(JToken token, TimeZoneInfo localTimeZone, bool wasModified, bool modified)
        {
            foreach (JToken item in token.Children())
            {
                JToken child = item;
                if (child.HasValues)
                {
                    modified = child.ConvertLocalToUtc(localTimeZone, wasModified) || modified;
                }
            }

            return modified;
        }

        private static bool ConvertUtcToLocalForObject(
            JToken token,
            TimeZoneInfo localTimeZone,
            bool wasModified,
            bool modified)
        {
            foreach (JProperty prop in token.Children<JProperty>())
            {
                JToken child = prop.Value;
                if (child is JValue jValue)
                {
                    object value = ParseJsonValueForDateTime(jValue.Value);
                    if (value is DateTime utc)
                    {
                        // Only convert if Kind is unspecified and the property name
                        // does not end in "Date" (i.e., it's a date/time, not just a date)
                        if (utc.Kind == DateTimeKind.Unspecified && !prop.Name.EndsWith("Date"))
                        {
                            TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(localTimeZone.Id);
                            DateTime local = TimeZoneInfo.ConvertTimeFromUtc(utc, tz);
                            jValue.Value = local;
                            modified = true;
                        }
                    }
                    else if (prop.Name.EndsWith("Json") && value is string)
                    {
                        // Also handle JSON "embedded" in the response; i.e., string properties that contain JSON
                        string stringValue = value.ToString();
                        JToken embeddedJObject = stringValue.TryDeserializeToJToken();
                        if (embeddedJObject != null)
                        {
                            if (embeddedJObject.ConvertUtcToLocal(localTimeZone))
                            {
                                jValue.Value = JsonConvert.SerializeObject(embeddedJObject,
                                    new StringEnumConverter(),
                                    new CustomDateTimeConverter(dateFormat: null, dateTimeStyles: DateTimeStyles.AssumeLocal)
                                    );
                                modified = true;
                            }
                        }
                    }
                }
                else if (child.HasValues)
                {
                    modified = child.ConvertUtcToLocal(localTimeZone, wasModified) || modified;
                }
            }

            return modified;
        }

        private static bool ConvertUtcToLocalForArray(JToken token, TimeZoneInfo localTimeZone, bool wasModified, bool modified)
        {
            foreach (JToken item in token.Children())
            {
                JToken child = item;
                if (child.HasValues)
                {
                    modified = child.ConvertUtcToLocal(localTimeZone, wasModified) || modified;
                }
            }

            return modified;
        }

        private static object ParseJsonValueForDateTime(object value)
        {
            // If a date/time value includes seconds, it will be cast as a DateTime automatically
            // But if it's missing seconds, it will be treated as a string that we'll have to convert to a DateTime

            if (value is string)
            {
                string stringValue = value.ToString();

                if (stringValue.FromDateTimeIsoString(out DateTime dateTimeValue))
                {
                    value = dateTimeValue;
                }
            }

            return value;
        }
    }
}
