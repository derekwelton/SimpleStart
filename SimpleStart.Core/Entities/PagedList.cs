using System.Collections.Generic;

namespace SimpleStart.Core.Entities
{
    public sealed class PagedList<T>
    {
        public long TotalPages { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public List<T> Data { get; set; } = new List<T>();

        public PagedList()
        {
            
        }
        public PagedList(List<T> list, long pages, int pageSize, int pageNumber)
        {
            Data = list;
            TotalPages = pages;
            PageSize = pageSize;
            PageNumber = pageNumber;
        }
    }
}