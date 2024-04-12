using Calculation.Intrinsic.DataSets.GrahamIntrinsicModel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculation.Intrinsic.GrahamIntrinsicModel.Commands
{
    public class GrahamIntrinsicModelCommand : BaseIntrinsicModelCommand, IRequest<GrahamIntrinsicModelDataSet>
    {
        public GrahamIntrinsicModelCommand(string ticker, decimal currentPrice) : base(ticker, currentPrice) { }
        public decimal Eps { get; set; }
        public decimal FiveYearGrowth { get; set; }
        public decimal AverageBondYield { get; set; }
        public decimal CurrentBondYield { get; set; }
    }
}
