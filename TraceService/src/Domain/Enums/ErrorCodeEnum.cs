namespace TraceService.Domain.Enums
{
    public enum ErrorCodeEnum : int
    {
        GeneralError = 0,
        NoData = 1,
        MemoryFull = 2,
        SqlError = 3,
        InvalidInputParam = 4,
        InvalidData = 5,
        NotFound = 6,
        UnAuthorized = 7,
        Forbidden = 8
    }
}
