using System;
using System.Collections.Generic;
using TraceService.Application.Exceptions;
using TraceService.Application.Models;
using TraceService.Domain.Enums;
using TraceService.WebAPI.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TraceService.WebAPI.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

        public ApiExceptionFilterAttribute()
        {
            // Register known exception types and handlers.
            this._exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(ValidationException), this.HandleValidationException },
                { typeof(NotFoundException), this.HandleNotFoundException },
                { typeof(UnauthorizedAccessException), this.HandleUnauthorizedAccessException },
                { typeof(ForbiddenAccessException), this.HandleForbiddenAccessException },
                { typeof(BadRequestException), this.HandleBadRequestException },
                { typeof(InternalErrorException), this.HandleInternalErrorException },
            };
        }

        public override void OnException(ExceptionContext context)
        {
            this.HandleException(context);

            base.OnException(context);
        }


        private void HandleException(ExceptionContext context)
        {
            Type type = context.Exception.GetType();
            if (this._exceptionHandlers.TryGetValue(type, out Action<ExceptionContext> value))
            {
                value.Invoke(context);
                return;
            }

            if (!context.ModelState.IsValid)
            {
                HandleInvalidModelStateException(context);
                return;
            }

            HandleUnknownException(context);
        }

        private void HandleValidationException(ExceptionContext context)
        {
            ValidationException exception = context.Exception as ValidationException;
            IList<object> errors =  new List<object>(exception.Errors);   
            context.Result = new BadRequestObjectResult(new ObjectErrorModel(errors));
            context.ExceptionHandled = true;
        }

        private static void HandleInvalidModelStateException(ExceptionContext context)
        {
            //var details = new ValidationProblemDetails(context.ModelState);
            //{
            //    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            //};
            ObjectErrorModel details = new(new ErrorModel(context.Exception.Message, ErrorCodeEnum.InvalidData));
            context.Result = new BadRequestObjectResult(details);
            context.ExceptionHandled = true;
        }

        private void HandleNotFoundException(ExceptionContext context)
        {
            NotFoundException exception = context.Exception as NotFoundException;
            ObjectErrorModel details = new(new ErrorModel(exception.Message, ErrorCodeEnum.NotFound));
            context.Result = new NotFoundObjectResult(details);
            context.ExceptionHandled = true;
        }

        private void HandleUnauthorizedAccessException(ExceptionContext context)
        {
            UnauthorizedAccessException exception = context.Exception as UnauthorizedAccessException;
            ObjectErrorModel details = new(new ErrorModel(exception.Message, ErrorCodeEnum.UnAuthorized));
            context.Result = new ObjectResult(details) { StatusCode = StatusCodes.Status401Unauthorized };
            context.ExceptionHandled = true;
        }

        private void HandleForbiddenAccessException(ExceptionContext context)
        {
            ObjectErrorModel details = new(new ErrorModel(context.Exception.Message, ErrorCodeEnum.Forbidden));
            context.Result = new ObjectResult(details) { StatusCode = StatusCodes.Status403Forbidden };
            context.ExceptionHandled = true;
        }

        private static void HandleUnknownException(ExceptionContext context)
        {
            ObjectErrorModel details = new(new ErrorModel(context.Exception.Message, ErrorCodeEnum.GeneralError));
            context.Result = new ObjectResult(details) { StatusCode = StatusCodes.Status500InternalServerError };

            context.ExceptionHandled = true;
        }

        private void HandleBadRequestException(ExceptionContext context)
        {
            ObjectErrorModel details = new(new ErrorModel(context.Exception.Message, ErrorCodeEnum.InvalidInputParam));
            context.Result = new ObjectResult(details) { StatusCode = StatusCodes.Status400BadRequest };
            context.ExceptionHandled = true;
        }

        private void HandleInternalErrorException(ExceptionContext context)
        {
            InternalErrorException exception = context.Exception as InternalErrorException;
            ObjectErrorModel details = new(exception.Error);
            context.Result = new ObjectResult(details) { StatusCode = ErrorCodeStatusFactory.GetStatusCode((ErrorCodeEnum)exception.Error.Code) };
            context.ExceptionHandled = true;
        }

        
    }
}
