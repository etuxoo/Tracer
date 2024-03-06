using System;

namespace TraceService.Domain.Entities
{
    public class TraceEntity
    {
        public long Id { get; set; }
        public DateTime Dt { get; set; }
        public string? Mti { get; set; }
        public string? Trn { get; set; }
        //public long Duration { get; set; }
    }
}
