using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntrinsicValue.Calculation.Common.Services
{
    public class PostValuationService : IPostValuationService
    {
        public decimal CalculateBuyPrice(decimal intrinsicValue, decimal safetyMargin) =>
            Math.Round(intrinsicValue * safetyMargin, 2);

        public decimal CalculatePriceDifference(decimal buyPrice, decimal currentPrice)
        {
            decimal priceDifference = buyPrice > 0 ? Math.Round(currentPrice - buyPrice, 2) : Math.Round(currentPrice + Math.Abs(buyPrice), 2);
            if (currentPrice > buyPrice)
                return priceDifference * -1;

            return priceDifference;
        }

        public decimal CalculatePriceDifferencePercent(decimal priceDifference, decimal currentPrice) =>
            Math.Round(priceDifference / currentPrice * 100, 2);
    }
}
