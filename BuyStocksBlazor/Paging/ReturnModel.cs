using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paging
{
    public class ReturnModel
    {
        public int TotalRecordCount { get; set; }
        public int Page { get; set; }
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
       
        public string SortExpressionParameterName { get; set; }
        public string SortExpression { get; set; }

        public string DefaultSortExpression { get; set; }
        public IList Values { get; set; }
    }
}
