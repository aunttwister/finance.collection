using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.Common.CustomDataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finance.Collection.Domain.FinanceScraper.DataSets.Base;

namespace Finance.Collection.Domain.FinanceScraper.DataSets
{
    public class BalanceSheetDataSet : IFinanceDataSet
    {
        public MethodResult<decimal> TTMCashEquivalents { get; set; }
        public MethodResult<decimal> TTMTotalDebt { get; set; }
        public MethodResultDictionary<string, decimal> HistoricalCashEquivalents { get; set; }
        public MethodResultDictionary<string, decimal> HistoricalTotalDebt { get; set; }
    }
}
