using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Collection.Domain.IntrinsicValue.Calculation.DataSets.DCFIntrinsicModel
{
    public class LiabilitiesDataSet
    {
        public LiabilitiesDataSet(decimal ttmTotalDebt, IDictionary<string, decimal> historicalTotalDebt)
        {
            TTMTotalDebt = ttmTotalDebt;
            HistoricalTotalDebt = historicalTotalDebt;
        }
        public decimal TTMTotalDebt { get; set; }
        public IDictionary<string, decimal> HistoricalTotalDebt { get; set; }
    }
}
