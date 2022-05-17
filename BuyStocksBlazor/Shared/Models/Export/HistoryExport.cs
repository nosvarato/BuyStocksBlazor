using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyStocksBlazor.Shared.Models.Export
{
    public class HistoryExport
    {
        public string Purchased { get; set; }
        public string Symbol { get; set; }
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        [Display(Name = "Amount Purchased")]
        public int AmountPurchased { get; set; }
        
        [Display(Name = "Price per stock")]
        public decimal PricePerStock { get; set; }

        public decimal Total { get; set; }
    }
}
