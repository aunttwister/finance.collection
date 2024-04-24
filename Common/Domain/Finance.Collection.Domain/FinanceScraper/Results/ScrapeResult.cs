using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Collection.Domain.FinanceScraper.Results
{
    public class ScrapeResult
    {
        public string Ticker { get; set; }
        public decimal CurrentPrice { get; set; }
        public GrahamIntrinsicScrapeResult GrahamIntrinsicScrapeResult { get; set; }
        public DCFIntrinsicScrapeResult DCFIntrinsicScrapeResult { get; set; }

    }
}
