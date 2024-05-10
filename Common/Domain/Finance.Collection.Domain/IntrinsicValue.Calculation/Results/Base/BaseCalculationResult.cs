using IntrinsicValue.Calculation.DataSets.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Collection.Domain.IntrinsicValue.Calculation.Results.Base
{
    public class BaseCalculationResult
    {
        public BaseCalculationResult(decimal intrinsicValue, decimal buyPrice, decimal priceDifference, decimal priceDifferencePercent, decimal safetyMargin)
        {
            IntrinsicValue = intrinsicValue;
            BuyPrice = buyPrice;
            PriceDifference = priceDifference;
            PriceDifferencePercent = priceDifferencePercent;
            SafetyMargin = safetyMargin;
        }
        public decimal IntrinsicValue { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal PriceDifference { get; set; }
        public decimal PriceDifferencePercent { get; set; }
        public decimal SafetyMargin { get; set; }
    }
}
