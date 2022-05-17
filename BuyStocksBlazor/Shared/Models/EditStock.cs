using BuyStocksBlazor.Shared.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyStocksBlazor.Shared.Models
{
    public class EditStock
    {
        
        public Stock CurrentStock { get; set; } = null!;
        [Required]
        [TotalValidationAttribute]
        [Display(Name = "Amount to Purchase")]
        public int Amount { get; set; }

        [Display(Name = "Total Order Amount")]
        public decimal Total { get
            {
                return CurrentStock.CurrentPrice * Amount;
            }
            

        }
        [Display(Name = "Your Current Balance")]
        public decimal CurrentBalance { get; set; }
    }
}
