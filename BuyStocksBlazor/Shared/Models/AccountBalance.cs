using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyStocksBlazor.Shared.Models
{
    public class AccountBalance
    {
        public int AccountBalanceID { get; set; }
        public string UserId { get; set; }
        public decimal CurrentBalance { get; set; }
    }
}
