using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceScraper.Common.DataSets.Base;
using FinanceScraper.Common.DataType;

namespace FinanceScraper.Common.DataSets
{
    public class CashFlowDataSet : IFinanceDataSet
    {
        public DictionaryWithKeyValuePairExceptions<string, decimal> HistoricalYearCashFlows { get; set; }
    }
}
