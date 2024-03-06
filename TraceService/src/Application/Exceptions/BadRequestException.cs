using System;

namespace TraceService.Application.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string exceptionMessage)
            : base(exceptionMessage)
        {
        }
    }
}
