using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paging
{
    public class PagingList<T> : List<T>, IPagingList<T> 
    {

        public int PageIndex { get; }
        public int PageCount { get; }
        public int TotalRecordCount { get; }
        public string Action { get; set; }
        public string PageParameterName { get; set; }
        public string SortExpressionParameterName { get; set; }
        public string SortExpression { get; }

        public string DefaultSortExpression { get; }

   

        internal PagingList(List<T> list, int pageIndex, int pageCount, int totalRecordCount)
            : base(list)
        {
            this.TotalRecordCount = totalRecordCount;
            this.PageIndex = pageIndex;
            this.PageCount = pageCount;
            this.Action = "Index";
            this.PageParameterName = PagingOptions.Current.PageParameterName;
            this.SortExpressionParameterName = PagingOptions.Current.SortExpressionParameterName;
        }

        internal PagingList(List<T> list, int pageIndex, int pageCount, string sortExpression, string defaultSortExpression, int totalRecordCount)
            : this(list, pageIndex, pageCount, totalRecordCount)
        {

            this.SortExpression = sortExpression;
            this.DefaultSortExpression = defaultSortExpression;
        }

        

  

        public int NumberOfPagesToShow { get; set; } = PagingOptions.Current.DefaultNumberOfPagesToShow;

        public int StartPageIndex
        {
            get
            {
                var half = (int)((this.NumberOfPagesToShow - 0.5) / 2);
                var start = Math.Max(1, this.PageIndex - half);
                if (start + this.NumberOfPagesToShow - 1 > this.PageCount)
                {
                    start = this.PageCount - this.NumberOfPagesToShow + 1;
                }
                return Math.Max(1, start);
            }
        }

        public int StopPageIndex => Math.Min(this.PageCount, this.StartPageIndex + this.NumberOfPagesToShow - 1);

    }
}