using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntrinsicValue.Calculation.DataSets.DCFIntrinsicModel
{
    public class DCFIntrinsicModelDataSet : BaseIntrinsicModelDataSet
    {
        public decimal DiscountedCashFlowValue { get; set; }
        public decimal EquityValue { get; set; }
        public decimal SharesOutstanding { get; set; }
        public decimal CashAndCashEquivalents { get; set; }
        public decimal TotalDebt { get; set; }
        public decimal SumPresentFutureCashFlowValue { get; set; }
        public ICollection<FutureCashFlowDataSet> FutureCashFlowDataSet { get; set; }
        public IDictionary<string, decimal> FutureCashFlows { get; set; }
        public AverageGrowthRateDataSet AverageGrowthRateDataSet { get; set; }
        public IDictionary<string, decimal> HistoricalGrowthRates { get; set; }
        public IDictionary<string, decimal> HistoricalCashFlows { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal PerpetualRate { get; set; }
    }
}
