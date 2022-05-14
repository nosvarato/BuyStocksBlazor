using BuyStocksBlazor.Shared.Models;

namespace BuyStocksBlazor.Server.Data
{
    public class SeedData
    {
        private readonly ApplicationDBContext _applicationDBContext;

        public SeedData(ApplicationDBContext applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;
        }
        public async void Seed()
        {
            if (!_applicationDBContext.Stocks.Any())
            {
                var seedstock = new List<Stock>()
                {
                    new Stock() {Symbol="AAPL",CompanyName="Apple Inc.",LastPrice=147.11M},
                    new Stock() { Symbol = "MSFT", CompanyName = "Microsoft Corporation", LastPrice = 261.12M},
                    new Stock() { Symbol = "GOOG", CompanyName = "Alphabet Inc.", LastPrice = 2330.31M},
                    new Stock() { Symbol = "AMZN", CompanyName = "Amazon.com, Inc.", LastPrice = 2261.1M},
                    new Stock() { Symbol = "TSLA", CompanyName = "Tesla, Inc.", LastPrice = 769.59M},
                    new Stock() { Symbol = "FB", CompanyName = "Meta Platforms, Inc.", LastPrice = 198.62M},
                    new Stock() { Symbol = "NVDA", CompanyName = "NVIDIA Corporation", LastPrice = 177.06M},
                    new Stock() { Symbol = "BABA", CompanyName = "Alibaba Group Holding Limited", LastPrice = 87.99M},
                    new Stock() { Symbol = "INTC", CompanyName = "Intel Corporation", LastPrice = 43.6M},
                    new Stock() { Symbol = "CRM", CompanyName = "Salesforce, Inc.", LastPrice = 166.91M},
                    new Stock() { Symbol = "AMD", CompanyName = "Advanced Micro Devices, Inc.", LastPrice = 95.12M},
                    new Stock() { Symbol = "PYPL", CompanyName = "PayPal Holdings, Inc.", LastPrice = 78.83M},
                    new Stock() { Symbol = "ATVI", CompanyName = "Activision Blizzard, Inc.", LastPrice = 77.74M},
                    new Stock() { Symbol = "EA", CompanyName = "Electronic Arts Inc.", LastPrice = 124.94M},
                    new Stock() { Symbol = "TTD", CompanyName = "The Trade Desk, Inc.", LastPrice = 51.92M},
                    new Stock() { Symbol = "MTCH", CompanyName = "Match Group, Inc.", LastPrice = 77.51M},
                    new Stock() { Symbol = "ZG", CompanyName = "Zillow Group, Inc.", LastPrice = 40.14M},
                    new Stock() { Symbol = "YELP", CompanyName = "Yelp Inc.", LastPrice = 29.58M}

                };

                _applicationDBContext.AddRange(seedstock);
                await _applicationDBContext.SaveChangesAsync();
            }
        }
    }
}
