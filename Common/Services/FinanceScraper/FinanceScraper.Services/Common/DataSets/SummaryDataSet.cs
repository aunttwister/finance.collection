using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceScraper.Common.DataSets.Base;

namespace FinanceScraper.Common.DataSets
{
    public class SummaryDataSet : IFinanceDataSet
    {
        public decimal CurrentPrice { get; set; }
        public decimal Eps { get; set; }
    }
}
