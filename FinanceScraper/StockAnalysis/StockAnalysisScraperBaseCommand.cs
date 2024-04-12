using StockPortfolio.FinanceScraper.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPortfolio.FinanceScraper.StockAnalysis
{
    public class StockAnalysisScraperBaseCommand : ScraperBaseCommand
    {
        public StockAnalysisScraperBaseCommand(string ticker, string path) : base(ticker, path)
        {
            BaseUrl = "https://stockanalysis.com/stocks/";
            ResolveUrl();
        }
        public override void ResolveUrl()
        {
            SetFullUrl(BaseUrl + Ticker + Path);
        }
    }
}
