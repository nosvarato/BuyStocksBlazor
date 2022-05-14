using System;
using System.Collections.Generic;
using System.Text;

namespace Paging
{
    public interface IPagingList
    {
        string Action { get; set; }
        int PageCount { get; }
        int PageIndex { get; }
        int TotalRecordCount { get; }
        string SortExpression { get; }

        int NumberOfPagesToShow { get; set; }
        int StartPageIndex { get; }
        int StopPageIndex { get; }
    }

}
