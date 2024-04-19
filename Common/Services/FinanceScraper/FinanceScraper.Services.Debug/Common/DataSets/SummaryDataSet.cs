using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceScraper.Common.DataSets.Base;
using FinanceScraper.Common.Propagation;

namespace FinanceScraper.Common.DataSets
{
    public class SummaryDataSet : IFinanceDataSet
    {
        public MethodResult<decimal> CurrentPrice { get; set; }
        public MethodResult<decimal> Eps { get; set; }
    }
}
