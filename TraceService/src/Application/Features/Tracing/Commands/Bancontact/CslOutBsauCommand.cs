using Contracts.Interfaces.Bancontact;
using MediatR;
using TraceService.Application.Mappings;
using TraceService.Application.Models.Bancontact;
using TraceService.Application.Models;
using System;
using Newtonsoft.Json.Linq;

namespace TraceService.Application.Features.Tracing.Commands.Bancontact
{
    public class CslOutBsauCommand : IRequest<Result<CslOutBsauModel>>, IMapFrom<ICSLOutBSADRequest>
    {
        public long Id { get; set; }
        public DateTime Dt { get; set; }
        public string Mti { get; set; }
        public string Trn { get; set; }
        public string RrnCardScheme { get; set; }
        public string Message { get; set; }
    }
}
