using Newtonsoft.Json.Linq;

namespace Contracts.Interfaces.Bancontact
{
    public interface ICSLOutBSADRequest
    {
        public long Id { get; set; }
        public DateTime Dt { get; set; }
        public string? MTI { get; set; }
        public string? TRN { get; set; }
        public string? RRNCardScheme { get; set; }
        public string? Message { get; set; }
    }
}
