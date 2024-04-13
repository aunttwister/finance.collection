using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntrinsicValue.Calculation.DataSets.DCFIntrinsicModel
{
    public class FutureCashFlowDataSet
    {
        public string Year { get; set; }
        public decimal CashFlow { get; set; }
        public decimal PresentValue { get; set; }
    }
}
