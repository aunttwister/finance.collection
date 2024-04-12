using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockPortfolio.FinanceScraper.Common.DataSets.Base;

namespace StockPortfolio.FinanceScraper.Common.DataSets
{
    public class AnalysisDataSet : IFinanceDataSet
    {
        public decimal FiveYearGrowth { get; set; }
    }
}
