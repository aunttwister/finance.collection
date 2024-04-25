using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntrinsicValue.Calculation.DataSets.Common
{
    public class IntrinsicValueDataSet
    {
        public IntrinsicValueDataSet(decimal value)
        {
            Value = value;
            Date = DateTime.Now;
        }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
    }
}
