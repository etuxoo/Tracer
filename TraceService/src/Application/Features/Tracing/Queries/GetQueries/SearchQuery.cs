using System;
using MediatR;
using TraceService.Application.Models;

namespace TraceService.Application.Features.Tracing.Queries.GetQueries
{
    public class SearchQuery :  IRequest<Result<PaginatedList<SearchResultModel>>>
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string? Mid { get; set; }
        public string? Tid { get; set; }
        public string? Pan { get; set; }
        public string? ExpDate { get; set; }
        public string? Rrn { get; set; }
        public string? Mti { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
    }
}
