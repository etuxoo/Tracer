using System;
using System.Threading.Tasks;
using TraceService.Application.Models;

namespace TraceService.Application.Interfaces.Repositories
{
    public interface ISearchRepository
    {
        public Task<PaginatedList<SearchResultModel>> GetGeneralSearch(string mti, int PageSize, int Page, DateTime? dateFrom = null, DateTime? dateTo = null);
        public Task<PaginatedList<SearchResultModel>> GetCslInSearch(string mti, int PageSize, int Page, string mid, string tid, string panExpDate, DateTime? dateFrom = null, DateTime? dateTo = null);
        public Task<PaginatedList<SearchResultModel>> GetRrnSearch(string mti, string rrn, int PageSize, int Page, DateTime? dateFrom = null, DateTime? dateTo = null);
    }
}
