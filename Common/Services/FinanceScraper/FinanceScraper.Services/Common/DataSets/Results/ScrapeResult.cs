using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScraper.Common.DataSets.Results
{
    public class ScrapeResult
    {
        public string Ticker { get; set; }
        public decimal CurrentPrice { get; set; }
        public GrahamIntrinsicScrapeResult GrahamIntrinsicScrapeResult { get; set; }
        public DCFIntrinsicScrapeResult DCFIntrinsicScrapeResult { get; set; }

    }
}
