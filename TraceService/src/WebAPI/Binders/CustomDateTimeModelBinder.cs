using System;
using System.Linq;
using System.Threading.Tasks;
using TraceService.Application.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TraceService.WebAPI.Binders
{
    public class CustomDateTimeModelBinder : IModelBinder
    {
        public static readonly Type[] SUPPORTED_TYPES = new Type[] { typeof(DateTime), typeof(DateTime?) };

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            if (!SUPPORTED_TYPES.Contains(bindingContext.ModelType))
            {
                return Task.CompletedTask;
            }

            string modelName = this.GetModelName(bindingContext);

            ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

            string dateToParse = valueProviderResult.FirstValue;

            if (string.IsNullOrEmpty(dateToParse))
            {
                return Task.CompletedTask;
            }

            DateTime? dateTime = this.ParseDate(bindingContext, dateToParse);

            bindingContext.Result = ModelBindingResult.Success(dateTime);

            return Task.CompletedTask;
        }

        private DateTime? ParseDate(ModelBindingContext bindingContext, string dateToParse)
        {
            CustomDateTimeModelBinderAttribute attribute = this.GetDateTimeModelBinderAttribute(bindingContext);
            string dateFormat = attribute?.DateFormat;

            if (string.IsNullOrEmpty(dateFormat))
            {
                return DateTimeHelper.ParseDateTime(dateToParse);
            }

            return DateTimeHelper.ParseDateTime(dateToParse, new string[] { dateFormat });
        }

        private CustomDateTimeModelBinderAttribute GetDateTimeModelBinderAttribute(ModelBindingContext bindingContext)
        {
            string modelName = this.GetModelName(bindingContext);

            Microsoft.AspNetCore.Mvc.Abstractions.ParameterDescriptor paramDescriptor = bindingContext.ActionContext.ActionDescriptor.Parameters
                .Where(x => x.ParameterType == typeof(DateTime?))
                .Where((x) =>
                {
                    // See comment in GetModelName() on why we do this.
                    string paramModelName = x.BindingInfo?.BinderModelName ?? x.Name;
                    return paramModelName.Equals(modelName);
                })
                .FirstOrDefault();

            if (paramDescriptor is not ControllerParameterDescriptor ctrlParamDescriptor)
            {
                return null;
            }

            object attribute = ctrlParamDescriptor.ParameterInfo
                .GetCustomAttributes(typeof(CustomDateTimeModelBinderAttribute), false)
                .FirstOrDefault();

            return (CustomDateTimeModelBinderAttribute)attribute;
        }

        private string GetModelName(ModelBindingContext bindingContext)
        {
            return !string.IsNullOrEmpty(bindingContext.BinderModelName)
                ? bindingContext.BinderModelName
                : bindingContext.ModelName;
        }
    }

    public class CustomDateTimeModelBinderAttribute : ModelBinderAttribute
    {
        public string DateFormat { get; set; }

        public CustomDateTimeModelBinderAttribute()
            : base(typeof(CustomDateTimeModelBinder))
        {
        }
    }
}
