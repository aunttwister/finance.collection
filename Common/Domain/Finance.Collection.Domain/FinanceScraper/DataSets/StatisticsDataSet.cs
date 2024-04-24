using Finance.Collection.Domain.FinanceScraper.DataSets.Base;
using Finance.Collection.Domain.FinanceScraper.Propagation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Collection.Domain.FinanceScraper.DataSets
{
    public class StatisticsDataSet : IFinanceDataSet
    {
        public MethodResult<decimal> SharesOutstanding { get; set; }
    }
}
