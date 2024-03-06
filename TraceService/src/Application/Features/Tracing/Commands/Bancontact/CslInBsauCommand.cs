using System;
using Contracts.Interfaces.Bancontact;
using MediatR;
using Newtonsoft.Json.Linq;
using TraceService.Application.Mappings;
using TraceService.Application.Models;
using TraceService.Application.Models.Bancontact;

namespace TraceService.Application.Features.Tracing.Commands.Bancontact
{
    public class CslInBsauCommand : IRequest<Result<CslInBsauModel>>, IMapFrom<ICSLInBSAURequest>
    {
        public string Mid { get; set; }
        public string Tid { get; set; }
        public string RrnIn { get; set; }
        public string RrnOut { get; set; }
        public string ProcCode { get; set; }
        public string PanExpDate { get; set; }
        public string Message { get; set; }
        public long Id { get; set; }
        public DateTime Dt { get; set; }
        public string Mti { get; set; }
        public string Trn { get; set; }
    }
}
