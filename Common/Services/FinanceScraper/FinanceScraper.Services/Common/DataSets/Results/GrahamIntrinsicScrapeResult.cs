using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScraper.Common.DataSets.Results
{
    public class GrahamIntrinsicScrapeResult
    {
        public SummaryDataSet Summary { get; set; }
        public AnalysisDataSet Analysis { get; set; }
        public TripleABondsDataSet TripleABonds { get; set; }
    }
}
