using BuyStocksBlazor.Server.Models;
using BuyStocksBlazor.Shared.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BuyStocksBlazor.Server.Data
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
           
        }
        public DbSet<Stock> Stocks { get; set; } = null!;
        public DbSet<StockPurchase> StockPurchases { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<AccountBalance> AccountBalances { get; set; }
    }
}
