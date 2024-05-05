using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntrinsicValue.Calculation.Common.Services
{
    public interface IPostValuationService
    {
        decimal CalculateBuyPrice(decimal intrinsicValue, decimal safetyMargin);
        decimal CalculatePriceDifference(decimal buyPrice, decimal currentPrice);
        decimal CalculatePriceDifferencePercent(decimal priceDifference, decimal currentPrice);
    }
}
