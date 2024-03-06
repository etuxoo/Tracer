using System;
using TraceService.Application.Models;
using TraceService.Domain.Enums;

namespace TraceService.Application.Exceptions
{
    public class InternalErrorException : Exception
    {
        public InternalErrorException(string error, ErrorCodeEnum code = ErrorCodeEnum.GeneralError)
            : base("Internal error occured.")
        {
            this.Error = new(error, code);
        }

        public InternalErrorException(Result result)
        {
            ErrorCodeEnum code = (ErrorCodeEnum)Enum.Parse(typeof(ErrorCodeEnum), result.Code.ToString());

            if (!Enum.IsDefined(typeof(ErrorCodeEnum), code) && !code.ToString().Contains(","))
            {
                throw new InvalidOperationException($"{code} is not an underlying value of the ErrorCodeEnum enumeration.");
            }

            this.Error = new(result.Error, code);
        }

        public InternalErrorException(ErrorModel error)
        {
            this.Error = error;
        }

        public ErrorModel Error { get; }
    }
}
