using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntrinsicValue.Calculation.Interfaces
{
    public interface IGrahamIntrinsicModelService
    {
        public decimal CalculateInstrnisicValue(decimal eps, decimal fiveYearGrowth, decimal currentBondYield, decimal averageBondYield);
    }
}
