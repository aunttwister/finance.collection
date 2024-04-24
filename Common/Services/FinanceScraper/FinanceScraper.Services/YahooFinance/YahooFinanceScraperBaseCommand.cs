using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceScraper.Common.Base;
using FinanceScraper.Common.Constants;

namespace FinanceScraper.YahooFinance
{
    public abstract class YahooFinanceScraperBaseCommand : ScraperBaseCommand
    {
        public YahooFinanceScraperBaseCommand(string ticker) : base(ticker) 
        {
            BaseUrl = BaseUrlConstants.YahooFinance;
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
