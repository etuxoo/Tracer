using TraceService.Domain.Enums;


namespace TraceService.WebAPI.Extensions
{
    public static  class ErrorCodesExtensions
    {
        public static int GetStatusCode(this ErrorCodeEnum errorCode)
        {
            return ErrorCodeStatusFactory.GetStatusCode(errorCode);
        }

        public static int GetStatusCode(this int errorCode)
        {
            return ErrorCodeStatusFactory.GetStatusCode(errorCode);
        }
    }
}
