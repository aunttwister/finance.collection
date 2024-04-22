using IntrinsicValue.Calculation.DataSets.DCFIntrinsicModel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntrinsicValue.Calculation.DCFIntrinsicModel.Commands
{
    public class DCFIntrinsicModelCommand : BaseIntrinsicModelCommand, IRequest<DCFIntrinsicModelDataSet>
    {
        public DCFIntrinsicModelCommand(
            string ticker, 
            decimal currentPrice,
            Dictionary<string, decimal> historicalCashFlow,
            decimal ttmCashAndCashEquivalents,
            decimal ttmTotalDebt,
            decimal sharesOutstanding,
            decimal discountRate = 8m,
            decimal perpetualRate = 2.5m,
            decimal safetyMargin = 0.75m) : base(ticker, currentPrice) 
        { 
            HistoricalCashFlow = historicalCashFlow;
            TTMCashAndCashEquivalents = ttmCashAndCashEquivalents;
            TTMTotalDebt = ttmTotalDebt;
            SharesOutstanding = sharesOutstanding;
            DiscountRate = discountRate;
            PerpetualRate = perpetualRate;
            SafetyMargin = safetyMargin;
        }
        public DCFIntrinsicModelCommand(
            string ticker,
            decimal currentPrice,
            Dictionary<string, decimal> historicalCashFlow,
            decimal ttmCashAndCashEquivalents,
            decimal ttmTotalDebt,
            decimal sharesOutstanding,
            Dictionary<string, decimal> historicalCashAndCashEquivalents,
            Dictionary<string, decimal> historicalTotalDebt,
            decimal discountRate = 8m,
            decimal perpetualRate = 2.5m,
            decimal safetyMargin = 0.75m) : base(ticker, currentPrice)
        {
            HistoricalCashFlow = historicalCashFlow;
            TTMCashAndCashEquivalents = ttmCashAndCashEquivalents;
            TTMTotalDebt = ttmTotalDebt;
            SharesOutstanding = sharesOutstanding;
            DiscountRate = discountRate;
            PerpetualRate = perpetualRate;
            SafetyMargin = safetyMargin;
            HistoricalCashAndCashEquivalents = historicalCashAndCashEquivalents;
            HistoricalTotalDebt = historicalTotalDebt;
        }

        public Dictionary<string, decimal> HistoricalCashAndCashEquivalents { get; set; }
        public Dictionary<string, decimal> HistoricalTotalDebt { get; set; }
        public Dictionary<string, decimal> HistoricalCashFlow { get; set; }
        public decimal TTMCashAndCashEquivalents { get; set; }
        public decimal TTMTotalDebt { get; set; }
        public decimal SharesOutstanding { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal PerpetualRate { get; set; }
        public decimal SafetyMargin { get; set; }
    }
}
