using Newtonsoft.Json.Linq;

namespace TraceService.Domain.Entities.Bancontact
{
    public class CslOutBsad : TraceEntity
    {
        public string? RrnCardScheme { get; set; }
        public string? Message { get; set; }
    }
}
