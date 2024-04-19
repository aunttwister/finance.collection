using FinanceScraper.Common.DataSets.Base;
using FinanceScraper.Common.Propagation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScraper.Common.DataSets
{
    public class StatisticsDataSet : IFinanceDataSet
    {
        public MethodResult<decimal> SharesOutstanding { get; set; }
    }
}
