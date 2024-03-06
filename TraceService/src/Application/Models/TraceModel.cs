using System;
using TraceService.Application.Mappings;
using TraceService.Domain.Entities;

namespace TraceService.Application.Models
{
    public class TraceModel  :IMapFrom<TraceEntity>
    {
        public long Id { get; set; }
        public DateTime Dt { get; set; }
        public string? MTI { get; set; }
        public string? TRN { get; set; }
        //public long Duration { get; set; }
    }
}
