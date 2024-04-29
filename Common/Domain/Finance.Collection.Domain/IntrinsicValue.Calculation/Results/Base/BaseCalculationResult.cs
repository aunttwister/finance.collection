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
        public BaseCalculationResult(decimal intrinsicValue)
        {
            IntrinsicValue = new IntrinsicValueDataSet(intrinsicValue);
        }
        public BaseCalculationResult(decimal currentPrice, decimal intrinsicValue, decimal safetyMargin)
        {
            CurrentPrice = currentPrice;
            SafetyMargin = safetyMargin;
            IntrinsicValue = new IntrinsicValueDataSet(intrinsicValue);
            BuyPrice = CalculateBuyPrice(intrinsicValue, safetyMargin);
            PriceDifference = CalculatePriceDifference(BuyPrice, currentPrice);
            PriceDifferencePercent = CalculatePriceDifferencePercent(PriceDifference, currentPrice);
        }
        public decimal CurrentPrice { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal PriceDifference { get; set; }
        public decimal PriceDifferencePercent { get; set; }
        public decimal SafetyMargin { get; set; }
        private decimal CalculateBuyPrice(decimal intrinsicValue, decimal safetyMargin) =>
            Math.Round(intrinsicValue * safetyMargin, 2);
        private decimal CalculatePriceDifference(decimal buyPrice, decimal currentPrice) =>
            buyPrice > 0 ? Math.Round(currentPrice - buyPrice, 2) : Math.Round(currentPrice + Math.Abs(buyPrice), 2);
        private decimal CalculatePriceDifferencePercent(decimal priceDifference, decimal currentPrice) =>
            Math.Round(priceDifference / currentPrice * 100, 2);
        public IntrinsicValueDataSet IntrinsicValue { get; set; }
    }
}
