using System;

namespace ParameterBinding.Api.Models
{
    public class PaginationFilter
    {
        public PaginationFilter()
            : this(1, 10)
        {}
        
        public PaginationFilter(int pageNumber, int pageSize)
        {
            PageNumber = Math.Max(pageNumber, 1);
            PageSize = Math.Clamp(pageSize, 1, 20);
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        
        public int Offset() => (PageNumber - 1) * PageSize;
        
    }
}