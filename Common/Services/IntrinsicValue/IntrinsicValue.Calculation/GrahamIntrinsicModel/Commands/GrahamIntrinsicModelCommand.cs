using Finance.Collection.Domain.FinanceScraper.Results;
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
            GrahamIntrinsicScrapeResult scrapeResult,
            decimal safetyMargin) : base(scrapeResult.Ticker, scrapeResult.Summary.CurrentPrice.Data) 
        {
            Eps = scrapeResult.Summary.Eps.Data;
            FiveYearGrowth = scrapeResult.Analysis.FiveYearGrowth.Data;
            AverageBondYield = scrapeResult.TripleABonds.HistoricalAverageTripleABond.Data;
            CurrentBondYield = scrapeResult.TripleABonds.CurrentTripleABond.Data;
            SafetyMargin = safetyMargin;
        }
        public decimal Eps { get; set; }
        public decimal FiveYearGrowth { get; set; }
        public decimal AverageBondYield { get; set; }
        public decimal CurrentBondYield { get; set; }
        public decimal SafetyMargin { get; set; }
    }
}
