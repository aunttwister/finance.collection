using Finance.Collection.Domain.FinanceScraper.CustomDataType;
using Finance.Collection.Domain.FinanceScraper.DataSets.Base;
using Finance.Collection.Domain.FinanceScraper.Propagation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Collection.Domain.FinanceScraper.DataSets
{
    public class BalanceSheetDataSet : IFinanceDataSet
    {
        public decimal TTMCashEquivalents { get; set; }
        public decimal TTMTotalDebt { get; set; }
        public MethodResultDictionary<string,decimal> HistoricalCashEquivalents { get; set; }
        public MethodResultDictionary<string, decimal> HistoricalTotalDebt { get; set; }
    }
}
