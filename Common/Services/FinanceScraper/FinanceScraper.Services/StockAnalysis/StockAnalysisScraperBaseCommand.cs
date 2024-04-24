using FinanceScraper.Common.Base;
using FinanceScraper.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScraper.StockAnalysis
{
    public class StockAnalysisScraperBaseCommand : ScraperBaseCommand
    {
        public StockAnalysisScraperBaseCommand(string ticker, string path) : base(ticker, path)
        {
            BaseUrl = BaseUrlConstants.StockAnalysis;
            ResolveUrl();
        }
        public override void ResolveUrl()
        {
            SetFullUrl(BaseUrl + Ticker + Path);
        }
    }
}
