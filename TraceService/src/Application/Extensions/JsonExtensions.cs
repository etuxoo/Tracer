using System.Text.Json;

namespace TraceService.Application.Extensions
{
    public static class JsonExtensions
    {
        public static string Localize(this JsonDocument document, string lang)
        {
            return document.RootElement.GetProperty(lang).GetString();
        }

        //https://stackoverflow.com/questions/62996999/convert-object-to-system-text-json-jsonelement    
        public static JsonElement ToJsonElement(this object jsonObject)
        {
            return JsonSerializer.SerializeToElement(jsonObject);
        }
    }
}
