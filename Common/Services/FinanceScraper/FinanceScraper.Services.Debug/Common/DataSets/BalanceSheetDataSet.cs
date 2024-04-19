using FinanceScraper.Common.DataSets.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScraper.Common.DataSets
{
    public class BalanceSheetDataSet : IFinanceDataSet
    {
        public decimal TTMCashEquivalents { get; set; }
        public decimal TTMTotalDebt { get; set; }
        public Dictionary<string,decimal> HistoricalCashEquivalents { get; set; }
        public Dictionary<string, decimal> HistoricalTotalDebt { get; set; }
    }
}
