using AutoMapper;
using Newtonsoft.Json.Linq;
using TraceService.Application.Features.Tracing.Commands.Bancontact;
using TraceService.Application.Mappings;
using TraceService.Domain.Entities.Bancontact;

namespace TraceService.Application.Models.Bancontact
{
    public class CslInBsadModel : TraceModel,IMapFrom<CslInBsad>
    {
        public string? MID { get; set; }
        public string? TID { get; set; }
        public string? RRNIn { get; set; }
        public string? RRNOut { get; set; }
        public string? ProcCode { get; set; }
        public string? PANExpDate { get; set; }
        public string? Message { get; set; }

        public void Mapping(Profile profile)
        {
            _ = profile.CreateMap<CslInBsad, CslInBsadModel>().ReverseMap();
            _ = profile.CreateMap<CslInBsadCommand, CslInBsadModel>().ReverseMap();
        }
    }
}
