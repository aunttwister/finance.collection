﻿using StockPortfolio.FinanceScraper.Common.DataSets.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPortfolio.FinanceScraper.Common.DataSets
{
    public class StatisticsDataSet : IFinanceDataSet
    {
        public decimal SharesOutstanding { get; set; }
    }
}