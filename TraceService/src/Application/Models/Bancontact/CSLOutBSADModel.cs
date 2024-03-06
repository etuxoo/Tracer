using AutoMapper;
using Newtonsoft.Json.Linq;
using TraceService.Application.Features.Tracing.Commands.Bancontact;
using TraceService.Application.Mappings;
using TraceService.Domain.Entities.Bancontact;

namespace TraceService.Application.Models.Bancontact
{
    public class CslOutBsadModel : TraceModel,IMapFrom<CslOutBsad>
    {
        public string? RRNCardScheme { get; set; }
        public string? Message { get; set; }

        public void Mapping(Profile profile)
        {
            _ = profile.CreateMap<CslOutBsad, CslOutBsadModel>().ReverseMap();
            _ = profile.CreateMap<CslOutBsadCommand, CslOutBsadModel>().ReverseMap();
        }
    }
}
