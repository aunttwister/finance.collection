using IntrinsicValue.Calculation.DataSets.Common;
using IntrinsicValue.Calculation.GrahamIntrinsicModel.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntrinsicValue.Calculation.DataSets.GrahamIntrinsicModel
{
    public class GrahamIntrinsicModelDataSet : BaseIntrinsicModelDataSet
    {
        public GrahamIntrinsicModelDataSet(
            GrahamIntrinsicModelCommand request, 
            decimal intrinsicValue,
            decimal safetyMargin = 0.65m)
        {
            Ticker = request.Ticker;
            CurrentPrice = request.CurrentPrice;
            Eps = request.Eps;
            FiveYearGrowth = request.FiveYearGrowth;
            AverageBondYield = request.AverageBondYield;
            CurrentBondYield = request.CurrentBondYield;
            IntrinsicValue = new IntrinsicValueDataSet(intrinsicValue);
            SafetyMargin = safetyMargin;
            BuyPrice = Math.Round(intrinsicValue * safetyMargin, 2);
            PriceDifference = BuyPrice > 0 ? Math.Round((BuyPrice - CurrentPrice) / BuyPrice * 100, 2) : Math.Round((BuyPrice - CurrentPrice) / BuyPrice * -100, 2);
        }
        public string Ticker { get; set; }
        public decimal Eps { get; set; }
        public decimal FiveYearGrowth { get; set; }
        public decimal AverageBondYield { get; set; }
        public decimal CurrentBondYield { get; set; }
        public IntrinsicValueDataSet IntrinsicValue { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal PriceDifference { get; set; }
        public decimal SafetyMargin { get; set; }
    }
}
