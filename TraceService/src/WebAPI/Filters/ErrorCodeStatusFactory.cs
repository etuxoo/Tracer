using TraceService.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace TraceService.WebAPI.Extensions
{
    public static class ErrorCodeStatusFactory
    {
        public static readonly Dictionary<ErrorCodeEnum, int> ErrorCodesStatusMapping = new()
        {
            { ErrorCodeEnum.GeneralError, StatusCodes.Status500InternalServerError },
            { ErrorCodeEnum.NoData, StatusCodes.Status404NotFound },
            { ErrorCodeEnum.MemoryFull, StatusCodes.Status507InsufficientStorage },
            { ErrorCodeEnum.SqlError, StatusCodes.Status500InternalServerError },
            { ErrorCodeEnum.InvalidInputParam, StatusCodes.Status400BadRequest },
        };

        public static int GetStatusCode(ErrorCodeEnum errorCode)
        {
            return ErrorCodesStatusMapping.TryGetValue(errorCode, out int statusCode) ? statusCode : StatusCodes.Status500InternalServerError;
        }

        public static int GetStatusCode(int errorCode)
        {
            try
            {
                return ErrorCodesStatusMapping.TryGetValue((ErrorCodeEnum)errorCode, out int statusCode) ? statusCode : StatusCodes.Status500InternalServerError;
            }
            catch
            {
                return StatusCodes.Status500InternalServerError;
            }
            
        }   
    }
}

