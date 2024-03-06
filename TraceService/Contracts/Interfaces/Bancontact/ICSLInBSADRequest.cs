using Newtonsoft.Json.Linq;

namespace Contracts.Interfaces.Bancontact
{
    public interface ICSLInBSADRequest
    {
        public string? MID { get; set; }
        public string? TID { get; set; }
        public string? RRNIn { get; set; }
        public string? RRNOut { get; set; }
        public string? ProcCode { get; set; }
        public string? PANExpDate { get; set; }
        public string? Message { get; set; }
        public long Id { get; set; }
        public DateTime Dt { get; set; }
        public string? MTI { get; set; }
        public string? TRN { get; set; }
    }
}
