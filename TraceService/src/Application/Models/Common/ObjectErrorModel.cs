using System.Collections.Generic;
using TraceService.Domain.Enums;
using Newtonsoft.Json;

namespace TraceService.Application.Models
{
    public class ObjectErrorModel
    {
        public ObjectErrorModel()
        {
        }

        public ObjectErrorModel(object error, object data = null)
        {
            this.Errors = new List<object>() { error };
            this.Data = data;
        }

        public ObjectErrorModel(IList<object> errors, object data = null)
        {
            this.Errors = errors;
            this.Data = data;
        }

        public IList<object> Errors { get; set; }

        [JsonProperty("data" /*, NullValueHandling = NullValueHandling.Ignore*/)]
        public object Data { get; set; }
    }
}
