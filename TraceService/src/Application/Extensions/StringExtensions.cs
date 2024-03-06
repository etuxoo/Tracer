using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TraceService.Application.Extensions
{
    public static class StringExtensions
    {
        public static string Empty(this string data)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            data = string.Empty;
            return data;
        }

        public static string EmptyIfNull(this string data)
        {
            data ??= string.Empty;

            return data;
        }

        public static int? ToNullableInt(this string s)
        {
            if (int.TryParse(s, out int i))
            {
                return i;
            }

            return null;
        }

        public static long? ToNullableLong(this string s)
        {
            if (long.TryParse(s, out long i))
            {
                return i;
            }

            return null;
        }

        public static bool? ToNullableBool(this string s)
        {
            if (bool.TryParse(s, out bool i))
            {
                return i;
            }

            return null;
        }

        public static string ToLowerFirst(this string str)
        {
            if (str == null)
            {
                return null;
            }

            if (str.Length > 1)
            {
                return char.ToLower(str[0]) + str[1..];
            }

            return str.ToLower();
        }

        public static DateTime? ToNullableDateTime(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return null;
            }

            return DateTime.Parse(s, CultureInfo.InvariantCulture);
        }

        public static bool FromDateTimeIsoString(this string value, out DateTime dateTime)
        {
            if (
                (value.Length == 16 || (value.Length == 19 && value[16] == ':')) &&
                value[4] == '-' &&
                value[7] == '-' &&
                value[13] == ':' &&
                DateTime.TryParse(value, out DateTime parsedDateTime)  // calls DateTime.TryParse only after passing the smell test
               )
            {
                dateTime = parsedDateTime;
                return true;
            }

            dateTime = DateTime.MinValue;
            return false;
        }

        //public static string ToDateTimeIsoString(this DateTime value) =>  value.ToString("yyyy-MM-dd HH:mm:ss");
        public static string ToDateTimeIsoString(this DateTime value)
        {
            return value.ToString("yyyy-MM-ddTHH:mm:ssZ");
        }

        public static JToken TryDeserializeToJToken(this string json)
        {
            if (json == null || (!json.StartsWith("[") && !json.StartsWith("{")))
            {
                return null;
            }

            // Try to get the JSON object from the request content
            JToken jToken = default;
            try
            {
                jToken = JsonConvert.DeserializeObject<JToken>(json);
            }
            catch
            {
                // Ignore the exception, returning null to indicate bad JSON
            }

            return jToken;
        }

        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return char.ToLowerInvariant(value[0]) + value[1..];
        }

        public static string EscapeSpecialSymbols(this string value, string replacement = " ")
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            Regex reg = new Regex("[*'\",_&#^@]");
            value = reg.Replace(value, replacement);

            return value;
        }
    }

    
}
