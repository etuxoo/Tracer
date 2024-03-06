using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TraceService.Application.Mappings
{
    public class DictionaryConverter : ITypeConverter<JsonElement, Dictionary<string, string>>
    {
        public Dictionary<string, string> Convert(JsonElement source, Dictionary<string, string> destination, ResolutionContext context)
        {
            return JsonSerializer.Deserialize<Dictionary<string, string>>(source);
        }
    }
}
