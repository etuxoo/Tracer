using Newtonsoft.Json.Linq;

namespace TraceService.Domain.Entities.Bancontact
{
    public class CslInBsad : TraceEntity
    {
        public string? Mid { get; set; }
        public string? TId { get; set; }
        public string? RrnIn { get; set; }
        public string? RrnOut { get; set; }
        public string? ProcCode { get; set; }
        public string? PanExpDate { get; set; }
        public string? Message { get; set; }
    }
}
