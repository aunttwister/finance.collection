using Finance.Collection.Domain.IntrinsicValue.Calculation.Results.DCFIntrinsicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Collection.Domain.Models
{
    public class GrowthRate : AuditableEntity
    {
        public GrowthRate()
        {
            HistoricalGrowthRate = new HashSet<HistoricalGrowthRateResultDataSet>();
        }
        public string Period { get; set; }
        public decimal AverageGrowthRate { get; set; }
        public ICollection<HistoricalGrowthRateResultDataSet> HistoricalGrowthRate { get; set; }
    }
}
