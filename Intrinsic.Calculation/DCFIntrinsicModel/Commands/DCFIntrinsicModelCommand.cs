using Calculation.Intrinsic.DataSets.DCFIntrinsicModel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculation.Intrinsic.DCFIntrinsicModel.Commands
{
    public class DCFIntrinsicModelCommand : BaseIntrinsicModelCommand, IRequest<DCFIntrinsicModelDataSet>
    {
        public DCFIntrinsicModelCommand(string ticker, decimal currentPrice) : base(ticker, currentPrice) {   }

        public Dictionary<string, decimal> HistoricalCashFlows { get; set; }
        public decimal CashAndCashEquivalents { get; set; }
        public decimal TotalDebt { get; set; }
        public decimal SharesOutstanding { get; set; }
    }
}
