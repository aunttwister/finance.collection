using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockPortfolio.FinanceScraper.Common.DataSets.Base;

namespace StockPortfolio.FinanceScraper.Common.DataSets
{
    public class CashFlowDataSet : IFinanceDataSet
    {
        public Dictionary<string, decimal> HistoricalYearCashFlows { get; set; }
    }
}
