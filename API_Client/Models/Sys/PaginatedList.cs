using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Models
{
    public class PaginatedList<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalCount { get; set; }
        public List<T> ResultData { get; set; } = new List<T>();

        public PaginatedList() : base()
        {
            PageIndex = 0;
            TotalPages = 1;
        }

        public PaginatedList(List<T> items, int pageIndex, int totalPage)
        {
            this.PageIndex = pageIndex;
            this.TotalPages = totalPage;
            this.ResultData = items;
        }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            TotalCount = count;
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.ResultData.AddRange(items);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }


        public static int ReturnPageNum(int count, int pageSize)
        {
            return count % pageSize == 0 ? count / pageSize : count / pageSize + 1;
        }
    }
}