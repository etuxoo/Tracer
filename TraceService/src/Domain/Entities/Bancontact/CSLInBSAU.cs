using Newtonsoft.Json.Linq;

namespace TraceService.Domain.Entities.Bancontact
{
    public class CslInBsau : TraceEntity
    {
        public string? Mid { get; set; }
        public string? Tid { get; set; }
        public string? RrnIn { get; set; }
        public string? RrnOut { get; set; }
        public string? ProcCode { get; set; }
        public string? PanExpDate { get; set; }
        public string? Message { get; set; }
    }
}
