using System;

namespace TraceService.Application.Exceptions
{
    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException(string messesage = default) : base(messesage) { }
    }
}
