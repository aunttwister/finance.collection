﻿using Finance.Collection.Domain.Common.Results;
using Finance.Collection.Domain.FinanceScraper.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Collection.Domain.FinanceScraper.Results
{
    public class GrahamScrapeResult : IScrapeResult, ICalculable, ISingleResult
    {
        public string Ticker { get; set; }
        public SummaryDataSet Summary { get; set; }
        public AnalysisDataSet Analysis { get; set; }
        public TripleABondsDataSet TripleABonds { get; set; }
    }
}
