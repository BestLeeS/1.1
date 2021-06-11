using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Sys
{
    public class SearchParams
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int Total { get; set; } = 0;
        public string QueryName { get; set; }
        public Guid InnerCode { get; set; }
    }
}
