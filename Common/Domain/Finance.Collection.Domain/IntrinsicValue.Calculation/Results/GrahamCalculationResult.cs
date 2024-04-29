using Finance.Collection.Domain.FinanceScraper.Results;
using Finance.Collection.Domain.IntrinsicValue.Calculation.Results;
using Finance.Collection.Domain.IntrinsicValue.Calculation.Results.Base;
using IntrinsicValue.Calculation.DataSets.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntrinsicValue.Calculation.DataSets.Results
{
    public class GrahamCalculationResult : BaseCalculationResult, ICalculationResult
    {
        public GrahamCalculationResult(
            string ticker,
            decimal intrinsicValue,
            decimal eps,
            decimal fiveYearGrowth,
            decimal averageBondYield,
            decimal currentBondYield) : base(intrinsicValue)
        {
            Ticker = ticker;
            Eps = eps;
            FiveYearGrowth = fiveYearGrowth;
            AverageBondYield = averageBondYield;
            CurrentBondYield = currentBondYield;
        }
        public GrahamCalculationResult(
            string ticker,
            decimal currentPrice,
            decimal intrinsicValue,
            decimal eps,
            decimal fiveYearGrowth,
            decimal averageBondYield,
            decimal currentBondYield,
            decimal safetyMargin) : base(currentPrice, intrinsicValue, safetyMargin)
        {
            Ticker = ticker;
            Eps = eps;
            FiveYearGrowth = fiveYearGrowth;
            AverageBondYield = averageBondYield;
            CurrentBondYield = currentBondYield;
        }
        public string Ticker { get; set; }
        public decimal Eps { get; set; }
        public decimal FiveYearGrowth { get; set; }
        public decimal AverageBondYield { get; set; }
        public decimal CurrentBondYield { get; set; }
    }
}
