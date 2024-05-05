using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Collection.Domain.IntrinsicValue.Calculation.Results.DCFIntrinsicModel
{
    public class HistoricalGrowthRateResultDataSet
    {
        public HistoricalGrowthRateResultDataSet() { }

        public string AveragePeriod { get; set; }
        public decimal AverageGrowthRate { get; set; }
        public decimal SafetyAverageGrowthRate { get; set; }
        public Dictionary<string, decimal> HistoricalGrowthRates { get; set; }
    }
}
