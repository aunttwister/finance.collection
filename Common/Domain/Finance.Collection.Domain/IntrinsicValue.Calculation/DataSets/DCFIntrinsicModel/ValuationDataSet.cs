using IntrinsicValue.Calculation.DataSets.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Collection.Domain.IntrinsicValue.Calculation.DataSets.DCFIntrinsicModel
{
    public class ValuationDataSet
    {
        public ValuationDataSet(decimal equityValue, decimal discountedCashFlowValue)
        {
            EquityValue = equityValue;
            DiscountedCashFlowValue = new IntrinsicValueDataSet(discountedCashFlowValue);
        }
        public decimal EquityValue { get; set; }
        public IntrinsicValueDataSet DiscountedCashFlowValue { get; set; }
    }
}
