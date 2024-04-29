using IntrinsicValue.Blazor.Model;

namespace IntrinsicValue.Blazor.MockData
{
    public static class DataSource
    {
        private static List<WatchlistDto> _watchlistLiss { get; set; }
        public static List<WatchlistDto> InitializeWatchlistData()
        {
            _watchlistLiss = new List<WatchlistDto>()
            {
                new WatchlistDto()
                {
                    Id = Guid.NewGuid(),
                    Name = "Main Portoflio",
                    IsPortfolio = true,
                    Tickers = new List<TickerDto>()
                    {
                        CreateRandomTicker("NVDA", 877.35m, 11.93m, 73.54m, 200.43m),
                        CreateRandomTicker("AAPL", 172.50m, 6.19m, 27.89m, 150.75m),
                        CreateRandomTicker("MSFT", 305.22m, 8.05m, 37.88m, 130.65m),
                        CreateRandomTicker("AMZN", 140.25m, 0.75m, 186.67m, 175.32m),
                        CreateRandomTicker("GOOGL", 2987.35m, 101.34m, 29.47m, 132.15m),
                        CreateRandomTicker("TSLA", 1000.50m, 3.45m, 289.86m, 240.28m),
                        CreateRandomTicker("IBM", 146.35m, 9.43m, 15.51m, 64.50m),
                        CreateRandomTicker("INTC", 48.77m, 4.23m, 11.53m, 98.67m),
                        CreateRandomTicker("AMD", 120.77m, 2.13m, 56.72m, 173.00m)
                    }
                },
                new WatchlistDto()
                {
                    Id = Guid.NewGuid(),
                    Name = "Potentials",
                    IsPortfolio = false,
                    Tickers = new List<TickerDto>()
                    {
                        CreateRandomTicker("FB", 295.77m, 11.85m, 24.99m, 165.00m),
                        CreateRandomTicker("NFLX", 480.52m, 6.45m, 74.60m, 135.00m),
                        CreateRandomTicker("BABA", 88.55m, 5.32m, 16.65m, 122.43m),
                        CreateRandomTicker("ORCL", 102.34m, 3.21m, 13.50m, 110.90m),
                        CreateRandomTicker("PYPL", 187.75m, 4.58m, 40.99m, 150.28m),
                        CreateRandomTicker("SAP", 144.50m, 1.77m, 18.23m, 96.75m),
                        CreateRandomTicker("CSCO", 55.22m, 2.43m, 21.88m, 60.21m),
                        CreateRandomTicker("SQ", 63.19m, 0.67m, 95.32m, 188.40m),
                        CreateRandomTicker("TWTR", 44.77m, 1.05m, 42.50m, 110.15m),
                        CreateRandomTicker("SNAP", 15.88m, -0.21m, -76.33m, 135.00m)
                    }
                }
            };

            return _watchlistLiss;
        }

        private static TickerDto CreateRandomTicker(string ticker, decimal basePrice, decimal baseEPS, decimal basePE, decimal baseRate)
        {
            Random rand = new Random();
            decimal priceChange = Math.Round((decimal)rand.NextDouble() * 20.0m - 10.0m, 2);  // Random change between -10 to +10%
            decimal epsChange = Math.Round((decimal)rand.NextDouble() * 4.0m - 2.0m);     // Random change between -2 to +2
            decimal peChange = Math.Round((decimal)rand.NextDouble() * 10.0m - 5.0m);     // Random change between -5 to +5

            return new TickerDto()
            {
                Ticker = ticker,
                CurrentPrice = Math.Round(basePrice + (basePrice * priceChange / 100), 2),
                EPS = baseEPS + epsChange,
                PE = basePE + peChange,
                SharesOutstanding = (long)(5000000000 + rand.Next(-1000000000, 1000000000)),
                TTMCashAndCashEquivalents = (long)(40000000 + rand.Next(-10000000, 10000000)),
                TTMTotalDebt = (long)(20000000 + rand.Next(-5000000, 5000000)),
                GrowthRate = new GrowthRateDto()
                {
                    Period = "2014 - 2023",
                    Rate = Math.Round(baseRate + (decimal)rand.NextDouble() * 50.0m - 25.0m, 2) // Random change between -25 to +25
                },
                EarningsDate = new EarningsDateDto()
                {
                    Status = "Confirmed",
                    Date = DateTime.Today.AddDays(rand.Next(-30, 30)) // Random date within the next or last 30 days
                },
                BenjaminGrahamModel = new BenjaminGrahamModelDto()
                {
                    Value = Math.Round(basePrice + (basePrice * priceChange / 100) - 50m, 2),
                    PriceDifference = priceChange,
                    PriceDifferencePercentage = Math.Round(priceChange / 100, 2)
                },
                DCFModel = new DCFModelDto()
                {
                    Value = Math.Round(basePrice * 2m + (basePrice * priceChange / 100) + 100m, 2),
                    PriceDifference = basePrice + priceChange,
                    PriceDifferencePercentage = Math.Round(priceChange + 100 / basePrice, 2)
                },
                AverageIntrinsic = new AverageIntrinsicDto()
                {
                    Value = Math.Round((basePrice + (basePrice * priceChange / 100) + basePrice * 2m + 100m) / 2m, 2),
                    PriceDifference = priceChange,
                    PriceDifferencePercentage = Math.Round(priceChange / 200m, 2)
                }
            };
        }
    }
}
