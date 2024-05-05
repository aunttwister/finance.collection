using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Collection.Domain.IntrinsicValue.Calculation.Results.DCFIntrinsicModel
{
    public class EstimatedCashFlowResultDataSet
    {
        public string Year { get; set; }
        public decimal EstimatedCashFlow { get; set; }
        public decimal EstimatedPresentValue { get; set; }
    }
}
