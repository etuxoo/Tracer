using System;
using AutoMapper;
using TraceService.Application.Features.Tracing.Queries.GetQueries;
using TraceService.Application.Mappings;

namespace TraceService.Application.Models
{
    public class SearchModel : IMapFrom<SearchQuery>
    {
        private int _page;
        private int _pageSize;

        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string? Mid { get; set; }
        public string? Tid { get; set; }
        public string? Pan { get; set; }
        public string? ExpDate { get; set; }
        public string? Rrn { get; set; }
        public string? Mti { get; set; }
 
        public int PageSize
        {
            get => this._pageSize;
            set => this._pageSize = this._pageSize == 0 ? 10 : value;
        }

        public int Page
        {
            get => this._page;
            set => this._page = value < 1 ? 1 : value;
        }
        public void Mapping(Profile profile)
        {
            _ = profile.CreateMap<SearchQuery, SearchModel>().ReverseMap();
        }
    }
}
