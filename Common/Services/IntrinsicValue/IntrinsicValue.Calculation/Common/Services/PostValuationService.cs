using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntrinsicValue.Calculation.Common.Services
{
    public class PostValuationService : IPostValuationService
    {
        public decimal CalculateBuyPrice(decimal intrinsicValue, decimal safetyMargin)
        {
            decimal baseValue = Math.Round(intrinsicValue * safetyMargin, 2);
            if (intrinsicValue < 0)
            {
                return baseValue + intrinsicValue;
            }
            else
            {
                return baseValue;
            }
        }

        public decimal CalculatePriceDifference(decimal buyPrice, decimal currentPrice)
        {
            decimal priceDifference;

            // When buyPrice is positive
            if (buyPrice > 0)
            {
                if (buyPrice > currentPrice)
                    priceDifference = Math.Round(buyPrice - currentPrice, 2);
                else if (buyPrice.Equals(currentPrice))
                    priceDifference = 0;
                else
                    priceDifference = currentPrice - buyPrice;
            }
            else
            {
                // When buyPrice is zero or negative
                priceDifference = Math.Round(currentPrice + Math.Abs(buyPrice), 2);
            }

            // Adjusting the sign based on currentPrice vs buyPrice
            if (currentPrice > buyPrice)
                priceDifference *= -1;

            return priceDifference;
        }

        public decimal CalculatePriceDifferencePercent(decimal priceDifference, decimal currentPrice) =>
            Math.Round(priceDifference / currentPrice, 2);
    }
}
