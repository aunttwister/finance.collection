using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceScraper.Common.DataSets.Base;
using FinanceScraper.Common.CustomDataType;
using FinanceScraper.Common.Propagation;

namespace FinanceScraper.Common.DataSets
{
    public class CashFlowDataSet : IFinanceDataSet
    {
        public MethodResultDictionary<string, decimal> HistoricalCashFlows { get; set; }
    }
}
