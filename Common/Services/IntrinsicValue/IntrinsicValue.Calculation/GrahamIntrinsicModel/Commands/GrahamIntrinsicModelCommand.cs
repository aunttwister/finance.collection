using IntrinsicValue.Calculation.DataSets.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntrinsicValue.Calculation.GrahamIntrinsicModel.Commands
{
    public class GrahamIntrinsicModelCommand : BaseIntrinsicModelCommand, IRequest<GrahamCalculationResult>
    {
        public GrahamIntrinsicModelCommand(
            string ticker, 
            decimal currentPrice,
            decimal eps,
            decimal fiveYearGrowth,
            decimal averageBondYield,
            decimal currentBondYield) : base(ticker, currentPrice) 
        {
            Eps = eps;
            FiveYearGrowth = fiveYearGrowth;
            AverageBondYield = averageBondYield;
            CurrentBondYield = currentBondYield;
        }
        public decimal Eps { get; set; }
        public decimal FiveYearGrowth { get; set; }
        public decimal AverageBondYield { get; set; }
        public decimal CurrentBondYield { get; set; }
    }
}
