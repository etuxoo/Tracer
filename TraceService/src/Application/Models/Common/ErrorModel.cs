using TraceService.Domain.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace TraceService.Application.Models
{
    public class ErrorModel
    {
        private int _code;

        public ErrorModel() { }

        public ErrorModel(string message, string errorCode, string details = null)
        {
            this.Message = message;
            if (int.TryParse(errorCode, out int code))
            {
                this.Code = code;
            }
            else
            {
                this.Code = (int)ErrorCodeEnum.InvalidInputParam;
            }

            this.Details = details;
        }

        public ErrorModel(string message, ErrorCodeEnum errorCode = ErrorCodeEnum.GeneralError, string details = null)
        {
            this.Message = message;
            this.Code = (int)errorCode;
            this.Details = details;
        }

        public ErrorModel(string message, int code, string details = null)
        {
            this.Code = code;
            this.Message = message;
            this.Details = details;
        }

        public int Code
        {
            get => this._code;
            set
            {
                ErrorCodeEnum enumCode = (ErrorCodeEnum)Enum.Parse(typeof(ErrorCodeEnum), value.ToString());
                if (!Enum.IsDefined(typeof(ErrorCodeEnum), enumCode))
                {
                    throw new InvalidOperationException($"{enumCode} is not an underlying value of the ErrorCodeEnum enumeration.");
                }

                this._code = (int)enumCode;
            }
        }

        public string Message { get; set; }

        //[JsonProperty("details", NullValueHandling = NullValueHandling.Ignore)]
        public string Details { get; set; }
    }
}
