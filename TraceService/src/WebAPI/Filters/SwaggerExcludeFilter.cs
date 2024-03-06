using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TraceService.Application.Extensions;
using TraceService.Application.Swagger;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TraceService.WebAPI.Filters
{
    public class SwaggerExcludeFilter : IOperationFilter, ISchemaFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            List<PropertyInfo> ignoredProperties = context.MethodInfo.GetParameters()
                .SelectMany(p => p.ParameterType.GetProperties()
                    .Where(prop => prop.GetCustomAttribute<SwaggerExcludeAttribute>() != null))
                .ToList();

            if (!ignoredProperties.Any())
            {
                return;
            }

            foreach (PropertyInfo property in ignoredProperties)
            {
                operation.Parameters = operation.Parameters
                    .Where(p => !p.Name.Equals(property.Name, StringComparison.InvariantCulture))
                    .ToList();
            }
        }

        #region ISchemaFilter Members

        public void Apply(OpenApiSchema schema, SchemaFilterContext schemaFilterContext)
        {
            if (schema.Properties.Count == 0)
            {
                return;
            }

            const BindingFlags bindingFlags = BindingFlags.Public |
                                              BindingFlags.NonPublic |
                                              BindingFlags.Instance;
            IEnumerable<MemberInfo> memberList = schemaFilterContext.Type
                                .GetFields(bindingFlags).Cast<MemberInfo>()
                                .Concat(schemaFilterContext.Type 
                                .GetProperties(bindingFlags));

            IEnumerable<string> excludedList = memberList.Where(m =>
                                                m.GetCustomAttribute<SwaggerExcludeAttribute>()
                                                != null)
                                         .Select(m =>
                                             m.GetCustomAttribute<JsonPropertyAttribute>()
                                              ?.PropertyName
                                              ?? m.Name.ToCamelCase());

            foreach (string excludedName in excludedList)
            {
                if (schema.Properties.ContainsKey(excludedName))
                {
                    schema.Properties.Remove(excludedName);
                }
            }
        }

        #endregion
    }
}
