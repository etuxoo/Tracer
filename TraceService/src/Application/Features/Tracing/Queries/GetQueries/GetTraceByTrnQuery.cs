using MediatR;
using TraceService.Application.Models.Bancontact;
using TraceService.Application.Models;

namespace TraceService.Application.Features.Tracing.Queries.GetQueries
{
    public class GetTraceByTrnQuery : IRequest<Result<BancontactTraceModel>>
    {
        public string Trn { get; set; }
    }
}
