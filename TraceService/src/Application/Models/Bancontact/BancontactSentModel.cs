using AutoMapper;
using TraceService.Application.Features.Tracing.Commands.Bancontact;
using TraceService.Application.Mappings;
using TraceService.Domain.Entities.Bancontact;

namespace TraceService.Application.Models.Bancontact
{
    public class BancontactSentModel : TraceModel,IMapFrom<BancontactSent>
    {
        public byte[]? Message { get; set; }

        public void Mapping(Profile profile)
        {
            _ = profile.CreateMap<BancontactSent, BancontactSentModel>().ReverseMap();
            _ = profile.CreateMap<BancontactSentCommand, BancontactSentModel>().ReverseMap();
        }
    }
}
