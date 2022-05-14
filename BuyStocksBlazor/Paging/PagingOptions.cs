using System;
using System.Collections.Generic;
using System.Text;

namespace Paging
{
    public class PagingOptions
    {

        private static PagingOptions _current = new PagingOptions();

        public static PagingOptions Current
        {
            get => _current;
            set => _current = value ?? throw new ArgumentNullException(nameof(Current), "PagingOptions must be set");
        }

        public string ViewName { get; set; } = "Bootstrap4";
        public string HtmlIndicatorUp { get; set; } = " <span>&#11015;</span>";
        public string HtmlIndicatorDown { get; set; } = " <span>&#11014;</span>";
        public int DefaultNumberOfPagesToShow { get; set; } = 5;
        public string PageParameterName { get; set; } = "page";
        public string SortExpressionParameterName { get; set; } = "sortExpression";

    }
}
