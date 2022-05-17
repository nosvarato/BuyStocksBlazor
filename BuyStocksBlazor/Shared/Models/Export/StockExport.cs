using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyStocksBlazor.Shared.Models.Export
{
    public class StockExport
    {
        public string Symbol { get; set; } = null!;
        public string CompanyName { get; set; } = null!;
        public decimal CurrentPrice { get; set; } = 0;
    }
}
