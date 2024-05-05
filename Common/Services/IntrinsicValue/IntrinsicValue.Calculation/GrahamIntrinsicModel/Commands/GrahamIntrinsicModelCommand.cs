using Finance.Collection.Domain.FinanceScraper.Results;
using Financial.Collection.Domain.DTOs;
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
            TickerDto tickerDto,
            AAABondDto aaaBondDto,
            decimal safetyMargin) : base(tickerDto.Symbol, tickerDto.CurrentPrice) 
        {
            Eps = tickerDto.EPS;
            FiveYearGrowth = tickerDto.ExpectedFiveYearGrowth;
            AverageBondYield = aaaBondDto.AverageYield;
            CurrentBondYield = aaaBondDto.CurrentYield;
            SafetyMargin = safetyMargin;
        }
        public decimal Eps { get; set; }
        public decimal FiveYearGrowth { get; set; }
        public decimal AverageBondYield { get; set; }
        public decimal CurrentBondYield { get; set; }
        public decimal SafetyMargin { get; set; }
    }
}
