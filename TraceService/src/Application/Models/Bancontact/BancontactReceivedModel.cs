using AutoMapper;
using TraceService.Application.Features.Tracing.Commands.Bancontact;
using TraceService.Application.Mappings;
using TraceService.Domain.Entities.Bancontact;

namespace TraceService.Application.Models.Bancontact
{
    public class BancontactReceivedModel : TraceModel,IMapFrom<BancontactReceived>
    {
        public byte[]? Message { get; set; }

        public void Mapping(Profile profile)
        {
            _ = profile.CreateMap<BancontactReceived, BancontactReceivedModel>().ReverseMap();
            _ = profile.CreateMap<BancontactReceivedCommand, BancontactReceivedModel>().ReverseMap();
        }
    }
}
