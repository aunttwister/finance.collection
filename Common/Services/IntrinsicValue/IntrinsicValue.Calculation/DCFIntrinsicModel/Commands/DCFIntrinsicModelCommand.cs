using Finance.Collection.Domain.FinanceScraper.Results;
using Financial.Collection.Domain.DTOs;
using IntrinsicValue.Calculation.DataSets.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntrinsicValue.Calculation.DCFIntrinsicModel.Commands
{
    public class DCFIntrinsicModelCommand : BaseIntrinsicModelCommand, IRequest<DCFCalculationResult>
    {
        public DCFIntrinsicModelCommand(
            TickerDto tickerDto,
            decimal safetyMargin,
            decimal discountRate = 0.08m,
            decimal perpetualRate = 0.025m) : base(tickerDto.Symbol, tickerDto.CurrentPrice)
        {
            HistoricalCashFlow = tickerDto.YearlyData
                .Where(data => data.CashFlow != 0m)
                .ToDictionary(data => data.Year, data => data.CashFlow);
            TTMCashAndCashEquivalents = tickerDto.TTMCashAndCashEquivalents;
            TTMTotalDebt = tickerDto.TTMTotalDebt;
            SharesOutstanding = tickerDto.SharesOutstanding;
            DiscountRate = discountRate;
            PerpetualRate = perpetualRate;
            SafetyMargin = safetyMargin;
        }
        public Dictionary<string, decimal> HistoricalCashFlow { get; set; }
        public decimal TTMCashAndCashEquivalents { get; set; }
        public decimal TTMTotalDebt { get; set; }
        public decimal SharesOutstanding { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal PerpetualRate { get; set; }
        public decimal SafetyMargin { get; set; }
    }
}
