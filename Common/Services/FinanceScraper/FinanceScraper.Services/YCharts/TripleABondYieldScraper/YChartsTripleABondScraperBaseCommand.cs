using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceScraper.Common.Base;
using FinanceScraper.Common.NodeResolver;

namespace FinanceScraper.YCharts.TripleABondYieldScraper
{
    public abstract class YChartsTripleABondScraperBaseCommand : ScraperBaseCommand
    {
        public YChartsTripleABondScraperBaseCommand() : base()
        {
            BaseUrl = "https://ycharts.com/";
            Path = "indicators/us_coporate_aaa_effective_yield#:~:text=Basic%20Info,long%20term%20average%20of%204.05%25.";
            ResolveUrl();
        }

        public override void ResolveUrl()
        {
            SetFullUrl(BaseUrl + Path);
        }
    }
}
