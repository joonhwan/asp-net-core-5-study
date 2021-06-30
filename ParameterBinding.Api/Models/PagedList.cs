using System.Collections.Generic;

namespace ParameterBinding.Api.Models
{
    public class PagedList<T>
    {
        public ICollection<T> Data { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int PageTotal { get; set; }
    }
}