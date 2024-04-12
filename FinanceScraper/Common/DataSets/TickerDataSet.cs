﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPortfolio.FinanceScraper.Common.DataSets
{
    public class TickerDataSet
    {
        public TickerDataSet() { }
        public string Ticker { get; set; }
        public decimal CurrentPrice { get; set; }
        public SummaryDataSet Summary { get; set; }
        public AnalysisDataSet Analysis { get; set; }
        public CashFlowDataSet CashFlow { get; set; }
    }
}
