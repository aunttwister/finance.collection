using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finance.Collection.Domain.Common.CustomDataType;
using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.FinanceScraper.DataSets.Base;

namespace Finance.Collection.Domain.FinanceScraper.DataSets
{
    public class CashFlowDataSet : IFinanceDataSet
    {
        public MethodResultDictionary<string, decimal> HistoricalCashFlows { get; set; }
    }
}
