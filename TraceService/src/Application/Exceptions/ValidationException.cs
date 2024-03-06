using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using TraceService.Application.Models;

namespace TraceService.Application.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException()
            : base("One or more validation failures have occurred.")
        {
        }

        public ValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            this.Errors = failures.Select(s => new ErrorModel(s.ErrorMessage, s.ErrorCode, s.PropertyName)).ToList();
        }

        public ValidationException(string propertyName, string message, string errorCode)
        {
            ErrorModel error = new(message, errorCode, propertyName);

            this.Errors = new List<ErrorModel>() { error };
        }

        public ValidationException(ValidationFailure failure)
        {
            ErrorModel error = new(failure.ErrorMessage, failure.ErrorCode, failure.PropertyName);

            this.Errors = new List<ErrorModel>() { error };
        }

        public IList<ErrorModel> Errors { get; }
    }
}
