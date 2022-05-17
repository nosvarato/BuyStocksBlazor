using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyStocksBlazor.Shared.Models
{
    public class StockPurchase
    {
        public int StockPurchaseId { get; set; }
        public DateTime Purchased { get; set; }
        public Stock StockPurchased { get; set; } = null!;
        public int StockId { get; set; }
        [Display(Name = "Amount Purchased")]
        public int AmountPurchased { get; set; }
        public string UserId { get; set; }
        [Display(Name = "Price per stock")]
        public decimal PricePerStock { get; set; }

    }
}
