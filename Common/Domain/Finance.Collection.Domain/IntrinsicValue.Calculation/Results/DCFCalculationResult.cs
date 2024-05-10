using Finance.Collection.Domain.IntrinsicValue.Calculation.Results.DCFIntrinsicModel;
using Finance.Collection.Domain.IntrinsicValue.Calculation.Results;
using Finance.Collection.Domain.IntrinsicValue.Calculation.Results.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finance.Collection.Domain.Common.Results;

namespace IntrinsicValue.Calculation.DataSets.Results
{
    public class DCFCalculationResult : BaseCalculationResult, ICalculationResult
    {
        public DCFCalculationResult(
            decimal intrinsicValue,
            decimal buyPrice,
            decimal priceDifference,
            decimal priceDifferencePercent,
            decimal equityValue,
            decimal safetyMargin,
            IEnumerable<EstimatedCashFlowResultDataSet> estimatedCashFlow,
            HistoricalGrowthRateResultDataSet historicalGrowthRate) : base(intrinsicValue, buyPrice, priceDifference, priceDifferencePercent, safetyMargin) 
        {
            EquityValue = equityValue;
            EstimatedCashFlows = estimatedCashFlow;
            HistoricalGrowthRate = historicalGrowthRate;
        }

        public decimal EquityValue { get; set; }
        public IEnumerable<EstimatedCashFlowResultDataSet> EstimatedCashFlows { get; set; }
        public HistoricalGrowthRateResultDataSet HistoricalGrowthRate { get; set; }
    }
}
