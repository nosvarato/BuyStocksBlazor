using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyStocksBlazor.Shared.Models
{
    public class CurrentUser
    {
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; } = null!;
        public Dictionary<string, string> Claims { get; set; } = null!;

        public decimal CurrentBalance { get; set; }

    }
}
