using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace BuyStocksBlazor.Client.Components.GridView
{
    public class SearchValues
    {
        public int Page { get; set; }
        public string SortExpression { get; set; }
        public string Filter { get; set; }
    }
}
