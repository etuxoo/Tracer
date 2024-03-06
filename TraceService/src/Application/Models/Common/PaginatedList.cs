using System;
using System.Collections.Generic;

namespace TraceService.Application.Models
{
    public class PaginatedList<T>
    {
        private int _pageSize;

        public int PageSize
        {
            get => this._pageSize;
            private set => this._pageSize = value > 100 ? 100 : value;
        }

        public IEnumerable<T> Items { get; }
        public int PageNumber { get; }
        public int TotalPages { get; }
        public int TotalCount { get; }

        public PaginatedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.TotalPages = pageSize != 0 ? (int)Math.Ceiling(count / (double)pageSize) : 0;
            this.TotalCount = count;
            this.Items = items;
        }

        public bool HasPreviousPage => this.PageNumber > 1;

        public bool HasNextPage => this.PageNumber < this.TotalPages;


        //Usefull in EF
        //public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        //{
        //    if (pageNumber <= 0)
        //    {
        //        throw new BadRequestException("Page number should not be less than 1!");
        //    }

        //    int count = await source.CountAsync();
        //    List<T> items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        //    return new(items, count, pageNumber, pageSize);
        //}
    }
}
