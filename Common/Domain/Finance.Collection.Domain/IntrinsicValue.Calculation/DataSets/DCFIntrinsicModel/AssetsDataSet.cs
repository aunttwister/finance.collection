using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Collection.Domain.IntrinsicValue.Calculation.DataSets.DCFIntrinsicModel
{
    public class AssetsDataSet
    {
        public AssetsDataSet(
            decimal ttmCashndCashEquivalents, 
            IDictionary<string, decimal> historicalCashAndCashEquivalents)
        {
            TTMCashAndCashEquivalents = ttmCashndCashEquivalents;
            HistoricalCashAndCashEquivalents = historicalCashAndCashEquivalents;
        }
        public decimal TTMCashAndCashEquivalents { get; set; }
        public IDictionary<string, decimal> HistoricalCashAndCashEquivalents { get; set; }
    }
}
