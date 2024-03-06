using System.Threading.Tasks;
using System.Threading;
using TraceService.Application.Interfaces.Repositories;
using TraceService.Application.Models;
using MediatR;
using TraceService.Application.Features.Tracing.Queries.GetQueries;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace TraceService.Application.Features.Tracing.Handlers
{
    public class SearchQueryHandler(ISearchRepository repository) : IRequestHandler<SearchQuery, Result<PaginatedList<SearchResultModel>>>
    {
        private readonly ISearchRepository _repository = repository;

        public async Task<Result<PaginatedList<SearchResultModel>>> Handle(SearchQuery request, CancellationToken token)
        {
            PaginatedList<SearchResultModel> data = !request.Mid.IsNullOrEmpty()
                ? await this._repository.GetCslInSearch(request.Mti,request.PageSize,request.Page,request.Mid,request.Tid,request.Pan,request.DateFrom,request.DateTo)
                : !request.Rrn.IsNullOrEmpty()
                    ? await this._repository.GetRrnSearch(request.Mti, request.Rrn, request.PageSize, request.Page, request.DateFrom, request.DateTo)
                    : await this._repository.GetGeneralSearch(request.Mti, request.PageSize, request.Page, request.DateFrom, request.DateTo);

            return !data.Items.Any() ?
                Result<PaginatedList<SearchResultModel>>.Failure("Missing Bancontact data") :
                Result<PaginatedList<SearchResultModel>>.Success(data);
        }

    }
}
