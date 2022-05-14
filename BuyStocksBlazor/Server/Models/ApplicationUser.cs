using Microsoft.AspNetCore.Identity;

namespace BuyStocksBlazor.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        public decimal CurrentBalance { get; set; }
    }
}
