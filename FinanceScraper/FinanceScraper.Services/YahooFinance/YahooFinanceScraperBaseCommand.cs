using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockPortfolio.FinanceScraper.Common.Base;

namespace StockPortfolio.FinanceScraper.YahooFinance
{
    public abstract class YahooFinanceScraperBaseCommand : ScraperBaseCommand
    {
        public YahooFinanceScraperBaseCommand(string ticker) : base(ticker) 
        {
            BaseUrl = "https://finance.yahoo.com/quote/";
            ResolveUrl();
        }
        public YahooFinanceScraperBaseCommand(string ticker, string path) : base(ticker, path)
        {
            BaseUrl = "https://finance.yahoo.com/quote/";
            Path = path;
            ResolveUrl();
        }

        public override void ResolveUrl()
        {
            SetFullUrl(BaseUrl + Ticker + Path + Ticker);
        }
    }
}
