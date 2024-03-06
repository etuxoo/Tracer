namespace Contracts.Interfaces.Bancontact
{
    public interface IBancontactSentRequest
    {
        public long Id { get; set; }
        public DateTime Dt { get; set; }
        public string? MTI { get; set; }
        public string? TRN { get; set; }
        public byte[]? Message { get; set; }
    }
}
