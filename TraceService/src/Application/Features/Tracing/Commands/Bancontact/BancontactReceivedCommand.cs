using System;
using Contracts.Interfaces.Bancontact;
using MediatR;
using TraceService.Application.Mappings;
using TraceService.Application.Models;
using TraceService.Application.Models.Bancontact;

namespace TraceService.Application.Features.Tracing.Commands.Bancontact
{
    public class BancontactReceivedCommand : IRequest<Result<BancontactReceivedModel>>, IMapFrom<IBancontactReceivedRequest>
    {
        public long Id { get; set; }
        public DateTime Dt { get; set; }
        public string Mti { get; set; }
        public string Trn { get; set; }
        public byte[] Message { get; set; }
    }
}
