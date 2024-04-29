using Finance.Collection.Domain.FinanceScraper.Constants;
using Finance.Collection.Domain.FinanceScraper.DataSets;
using Finance.Collection.Domain.FinanceScraper.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntrinsicValue.Calculation.Init.ExecutionStrategy;
using Finance.Collection.Domain.IntrinsicValue.Calculation.Results;
using Finance.Collection.Domain.Common.Propagation;
using IntrinsicValue.Calculation.DataSets.Results;
using IntrinsicValue.Calculation.DCFIntrinsicModel.Commands;
using IntrinsicValue.Calculation.GrahamIntrinsicModel.Commands;
using System.Diagnostics.CodeAnalysis;

namespace FinanceScraper.Common.Init.ExecutionStrategy
{
    public class GrahamCalculationExecutionStrategy : ICalculationExecutionStrategy
    {
        private readonly IMediator _mediator;
        private readonly string _ticker;
        public GrahamCalculationExecutionStrategy(IMediator mediator, string ticker)
        {
            _mediator = mediator;
            _ticker = ticker;
        }

        public async Task<MethodResult<ICalculationResult>> ExecuteCalculationStrategy(IScrapeResult scrapeResult, decimal safetyMargin)
        {
            GrahamIntrinsicScrapeResult grahamScrapeResult = (GrahamIntrinsicScrapeResult)scrapeResult;
            if (grahamScrapeResult.Ticker is null)
                grahamScrapeResult.Ticker = _ticker;
            GrahamIntrinsicModelCommand request = new GrahamIntrinsicModelCommand(grahamScrapeResult, safetyMargin);

            Task<GrahamCalculationResult> result = _mediator.Send(request);

            await result.ConfigureAwait(false);

            return new MethodResult<ICalculationResult>(result.Result);
        }
    }
}
