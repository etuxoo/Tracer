namespace TraceService.Application.Models.Bancontact
{
    public class BancontactTraceModel
    {
        public BancontactReceivedModel BancontactReceived { get; set; }
        public BancontactSentModel BancontactSent { get; set; }
        public CslInBsadModel CslInBsad { get; set; }
        public CslInBsauModel CslInBsau { get; set; }
        public CslOutBsadModel CslOutBsad { get; set; }
        public CslOutBsauModel CslOutBsau { get; set; }
    }
}
