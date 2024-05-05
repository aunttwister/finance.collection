using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceScraper.Common.Base;
using Finance.Collection.Domain.FinanceScraper.Constants;
using FinanceScraper.Common.NodeResolver;

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
            BaseUrl = BaseUrlConstants.YahooFinance;
            Path = path;
            ResolveUrl();
        }

        public override void ResolveUrl()
        {
            SetFullUrl(BaseUrl + Ticker + Path + Ticker);
        }
    }
}
