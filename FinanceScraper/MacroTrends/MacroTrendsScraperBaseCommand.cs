using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockPortfolio.FinanceScraper.Common.Base;

namespace StockPortfolio.FinanceScraper.MacroTrends
{
    public class MacroTrendsScraperBaseCommand : ScraperBaseCommand
    {
        public MacroTrendsScraperBaseCommand(string ticker, string path, string actionPath) : base(ticker)
        {
            BaseUrl = "https://www.macrotrends.net";
            Path = path;
            ActionPath = actionPath;
        }
        public string ActionPath { get; set; }

        public override void ResolveUrl()
        {
            SetFullUrl(BaseUrl + Path + Ticker);
        }
    }
}
