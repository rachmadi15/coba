using System.Collections.Generic;

namespace IDXWeb.Models
{
    public class Response<T>
    {
        public List<T> Items { get; set; }
        public int ItemCount { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int PageCount { get; set; }
    }
}