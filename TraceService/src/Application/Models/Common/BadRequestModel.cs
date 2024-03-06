using System.Collections.Generic;
using System.Linq;

namespace TraceService.Application.Models
{
    public class BadRequestModel : ErrorModel
    {
        public BadRequestModel(string message)
            : base(message)
        {
        }

        public BadRequestModel(string message, params string[] fields)
            : base(message)
        {
            this.Fields = fields.ToList();
        }

        public List<string> Fields { get; set; }
    }
}
