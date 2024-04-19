using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceScraper.Common.DataSets.Base;
using FinanceScraper.Common.Propagation;

namespace FinanceScraper.Common.DataSets
{
    public class TripleABondsDataSet : IFinanceDataSet
    {
        public MethodResult<decimal> CurrentTripleABond { get; set; }
        public MethodResult<decimal> HistoricalAverageTripleABond { get; set; }
    }
}
