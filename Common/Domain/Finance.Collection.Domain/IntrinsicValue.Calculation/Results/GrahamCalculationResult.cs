using Finance.Collection.Domain.FinanceScraper.Results;
using Finance.Collection.Domain.IntrinsicValue.Calculation.Results;
using Finance.Collection.Domain.IntrinsicValue.Calculation.Results.Base;
using IntrinsicValue.Calculation.DataSets.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntrinsicValue.Calculation.DataSets.Results
{
    public class GrahamCalculationResult : BaseCalculationResult, ICalculationResult
    {
        public GrahamCalculationResult(
            decimal intrinsicValue,
            decimal buyPrice,
            decimal priceDifference,
            decimal priceDifferencePercent,
            decimal safetyMargin) : base(intrinsicValue, buyPrice, priceDifference, priceDifferencePercent, safetyMargin) { }
    }
}
