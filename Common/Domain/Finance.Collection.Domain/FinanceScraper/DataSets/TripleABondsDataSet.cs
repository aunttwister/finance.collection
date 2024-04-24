using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finance.Collection.Domain.FinanceScraper.DataSets.Base;
using Finance.Collection.Domain.FinanceScraper.Propagation;

namespace Finance.Collection.Domain.FinanceScraper.DataSets
{
    public class TripleABondsDataSet : IFinanceDataSet
    {
        public MethodResult<decimal> CurrentTripleABond { get; set; }
        public MethodResult<decimal> HistoricalAverageTripleABond { get; set; }
    }
}
