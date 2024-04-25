using IntrinsicValue.Calculation.DataSets.DCFIntrinsicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Collection.Domain.IntrinsicValue.Calculation.DataSets.DCFIntrinsicModel
{
    public class GrowthRateDataSet
    {
        public GrowthRateDataSet()
        {
            HistoricalGrowthRate = new HashSet<HistoricalGrowthRateDataSet>();
        }
        public string Period { get; set; }
        public decimal AverageGrowthRate { get; set; }
        public ICollection<HistoricalGrowthRateDataSet> HistoricalGrowthRate { get; set; }
    }
}
