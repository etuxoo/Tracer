using Contracts.Interfaces.Bancontact;
using MediatR;
using TraceService.Application.Mappings;
using TraceService.Application.Models.Bancontact;
using TraceService.Application.Models;
using System;
using Newtonsoft.Json.Linq;

namespace TraceService.Application.Features.Tracing.Commands.Bancontact
{
    public class CslInBsadCommand : IRequest<Result<CslInBsadModel>>, IMapFrom<ICSLInBSADRequest>
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
