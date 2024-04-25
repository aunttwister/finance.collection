using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Collection.Domain.IntrinsicValue.Calculation.DataSets.DCFIntrinsicModel
{
    public class ConfigurationDataSet
    {
        public ConfigurationDataSet()
        {
            
        }
        public ConfigurationDataSet(
            decimal discountRate,
            decimal perpetualRate,
            decimal safetyMargin)
        {
            DiscountRate = discountRate;
            PerpetualRate = perpetualRate;
            SafetyMargin = safetyMargin;
        }
        public decimal DiscountRate { get; set; }
        public decimal PerpetualRate { get; set; }
        public decimal SafetyMargin { get; set; }
    }
}
