using Newtonsoft.Json.Linq;

namespace TraceService.Domain.Entities.Bancontact
{
    public class CslOutBsau : TraceEntity
    {
        public string? RrnCardScheme { get; set; }
        public string? Message { get; set; }
    }
}
