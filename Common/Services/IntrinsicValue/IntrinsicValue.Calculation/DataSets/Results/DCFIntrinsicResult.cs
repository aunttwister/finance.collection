using IntrinsicValue.Calculation.DataSets.Common;
using IntrinsicValue.Calculation.DataSets.DCFIntrinsicModel;
using IntrinsicValue.Calculation.DCFIntrinsicModel.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntrinsicValue.Calculation.DataSets.Results
{
    public class DCFIntrinsicResult : BaseIntrinsicModelDataSet
    {
        public DCFIntrinsicResult(
            DCFIntrinsicModelCommand request,
            decimal discountedCashFlow,
            decimal equity,
            decimal sumFutureCashFlow,
            List<FutureCashFlowDataSet> futureCashFlowDataSet,
            AverageGrowthRateDataSet averageGrowthRateDataSet,
            List<HistoricalGrowthRateDataSet> historicalGrowthRates)
        {
            //Data passed from request
            SharesOutstanding = request.SharesOutstanding;
            TTMCashAndCashEquivalents = request.TTMCashAndCashEquivalents;
            TTMTotalDebt = request.TTMTotalDebt;
            CurrentPrice = request.CurrentPrice;
            Ticker = request.Ticker;
            HistoricalCashFlow = request.HistoricalCashFlow;
            DiscountRate = request.DiscountRate;
            PerpetualRate = request.PerpetualRate;
            HistoricalCashAndCashEquivalents = request.HistoricalCashAndCashEquivalents;
            HistoricalTotalDebt = request.HistoricalTotalDebt;

            //Newly calculated data
            DiscountedCashFlowValue = new IntrinsicValueDataSet(discountedCashFlow);
            EquityValue = equity;
            SumFutureCashFlowValue = sumFutureCashFlow;
            FutureCashFlowDataSet = futureCashFlowDataSet;
            AverageGrowthRateDataSet = averageGrowthRateDataSet;
            HistoricalGrowthRate = historicalGrowthRates;

        }
        public IntrinsicValueDataSet DiscountedCashFlowValue { get; set; }
        public decimal EquityValue { get; set; }
        public decimal SharesOutstanding { get; set; }
        public decimal TTMCashAndCashEquivalents { get; set; }
        public decimal TTMTotalDebt { get; set; }
        public decimal SumFutureCashFlowValue { get; set; }
        public ICollection<FutureCashFlowDataSet> FutureCashFlowDataSet { get; set; }
        public AverageGrowthRateDataSet AverageGrowthRateDataSet { get; set; }
        public ICollection<HistoricalGrowthRateDataSet> HistoricalGrowthRate { get; set; }
        public IDictionary<string, decimal> HistoricalCashFlow { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal PerpetualRate { get; set; }
        public IDictionary<string, decimal> HistoricalCashAndCashEquivalents { get; set; }
        public IDictionary<string, decimal> HistoricalTotalDebt { get; set; }
    }
}
