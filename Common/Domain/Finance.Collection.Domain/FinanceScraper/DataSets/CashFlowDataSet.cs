using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finance.Collection.Domain.FinanceScraper.DataSets.Base;
using Finance.Collection.Domain.FinanceScraper.CustomDataType;
using Finance.Collection.Domain.FinanceScraper.Propagation;

namespace Finance.Collection.Domain.FinanceScraper.DataSets
{
    public class CashFlowDataSet : IFinanceDataSet
    {
        public MethodResultDictionary<string, decimal> HistoricalCashFlows { get; set; }
    }
}
