using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyStocksBlazor.Shared.Models
{
    public class Stock
    {
        public int StockId { get; set; }
        public string Symbol { get; set; } = null!;
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; } = null!;
        public decimal LastPrice { get; set; } = 0;
        [NotMapped]
        public decimal CurrentPrice { get
            {
                var RandomPercentVariance = (new Random().NextDouble() * 10) - 5.0;
                return LastPrice; /*+ (LastPrice * (decimal)RandomPercentVariance)/100;*/
            } 
        }
    }
}
