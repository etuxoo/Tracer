using AutoMapper;
using TraceService.Application.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TraceService.Infrastructure.Mappings
{
    public class MappingsProfile : Profile
    {
        public MappingsProfile()
        {
            this.ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            List<Type> types = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>))
                ).ToList();

            foreach (Type type in types)
            {
                object instance = Activator.CreateInstance(type);

                MethodInfo methodInfo = type.GetMethod("Mapping") ?? type.GetInterface("IMapFrom`1").GetMethod("Mapping");

                methodInfo?.Invoke(instance, new object[] { this });
            }
        }
    }
}
