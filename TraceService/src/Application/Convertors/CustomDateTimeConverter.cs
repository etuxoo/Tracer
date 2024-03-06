using System;
using System.Globalization;
using TraceService.Application.Extensions;
using TraceService.Application.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Utilities;

namespace TraceService.Application.Convertors
{
    //https://www.vickram.me/custom-datetime-model-binding-in-asp-net-core-web-api
    public class CustomDateTimeConverter : DateTimeConverterBase
    {

        private const string DefaultDateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        private readonly string _dateFormat = null;
        private readonly DateTimeConverterBase _innerConverter = null;
        private readonly DateTimeStyles _styles = DateTimeStyles.AssumeUniversal;

        public CustomDateTimeConverter()
           : this(dateFormat: null, dateTimeStyles: DateTimeStyles.AssumeUniversal) { }

        public CustomDateTimeConverter(string dateFormat = null, DateTimeStyles dateTimeStyles = DateTimeStyles.AssumeUniversal)
            : this(dateFormat, dateTimeStyles, innerConverter: new IsoDateTimeConverter()) { }

        public CustomDateTimeConverter(string dateFormat = null, DateTimeStyles dateTimeStyles = DateTimeStyles.AssumeUniversal,  DateTimeConverterBase innerConverter = null)
        {
            this._dateFormat = dateFormat;
            this._innerConverter = innerConverter ?? new IsoDateTimeConverter();
            this._styles = dateTimeStyles;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            bool isNullableType = DateTimeHelper.IsNullableType(objectType);

            if (reader.TokenType == JsonToken.Null)
            {
                if (isNullableType)
                {
                    return null;
                }

                throw new JsonSerializationException($"Cannot convert null value to {objectType}.");
            }

            if (reader.TokenType != JsonToken.String)
            {
                throw new JsonSerializationException($"Unexpected token parsing date. Expected {nameof(String)}, got {reader.TokenType}.");
            }

            string dateToParse = reader.Value.ToString();

            if (isNullableType && string.IsNullOrWhiteSpace(dateToParse))
            {
                return null;
            }

            if (string.IsNullOrEmpty(this._dateFormat))
            {
                return DateTimeHelper.ParseDateTime(dateToParse).Value.AdjustDateTimeStyle(DateTimeStyles.AssumeUniversal);
            }

            DateTime? dt = DateTimeHelper.ParseDateTime(dateToParse, new string[] { this._dateFormat });

            return dt.AdjustDateTimeStyle(DateTimeStyles.AssumeUniversal);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is DateTime dateTime)
            {
                dateTime = dateTime.AdjustDateTimeStyle(DateTimeStyles.AssumeUniversal);
                string format = string.IsNullOrEmpty(this._dateFormat) ? DefaultDateTimeFormat : this._dateFormat;
                writer.WriteValue(dateTime.ToString(format));
            }
            else
            {
                this._innerConverter.WriteJson(writer, value, serializer);
            }
        }

    }
}
