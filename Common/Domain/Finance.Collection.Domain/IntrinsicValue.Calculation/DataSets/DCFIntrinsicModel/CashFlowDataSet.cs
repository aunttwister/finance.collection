using IntrinsicValue.Calculation.DataSets.DCFIntrinsicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Collection.Domain.IntrinsicValue.Calculation.DataSets.DCFIntrinsicModel
{
    public class CashFlowDataSet
    {
        public IDictionary<string, decimal> HistoricalCashFlow { get; set; }
        public ICollection<FutureCashFlowPresentValueDataSet> FutureCashFlowPresentValue { get; set; }
        public decimal PresentValueSum { get; set; }
    }
}
