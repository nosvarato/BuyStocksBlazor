using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyStocksBlazor.Shared.Util
{
    public class Filter
    {
        public int Page { get; set; } = 1;
        public string SortExpression { get; set; } = null!;
        public string FilterValue { get; set; } = "";
    }
}
