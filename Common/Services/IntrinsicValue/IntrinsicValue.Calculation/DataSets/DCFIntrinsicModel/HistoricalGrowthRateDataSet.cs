using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntrinsicValue.Calculation.DataSets.DCFIntrinsicModel
{
    public class HistoricalGrowthRateDataSet
    {
        public HistoricalGrowthRateDataSet(
            string year,
            decimal cashFlow,
            decimal growthRate)
        {
            Year = year;
            CashFlow = cashFlow;
            GrowthRate = growthRate;
        }
        public HistoricalGrowthRateDataSet(
            string year,
            decimal cashFlow)
        {
            Year = year;
            CashFlow = cashFlow;
        }
        public string Year {  get; set; }
        public decimal CashFlow { get; set; }
        public decimal? GrowthRate { get; set; }
    }
}
