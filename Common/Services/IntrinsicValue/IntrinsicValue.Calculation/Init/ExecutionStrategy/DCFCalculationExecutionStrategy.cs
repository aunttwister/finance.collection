using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.FinanceScraper.Constants;
using Finance.Collection.Domain.FinanceScraper.Results;
using Finance.Collection.Domain.IntrinsicValue.Calculation.Results;
using IntrinsicValue.Calculation.DataSets.Results;
using IntrinsicValue.Calculation.DCFIntrinsicModel.Commands;
using MediatR;

namespace IntrinsicValue.Calculation.Init.ExecutionStrategy
{
    public class DCFCalculationExecutionStrategy : ICalculationExecutionStrategy
    {
        private readonly IMediator _mediator;
        private readonly string _ticker;
        /*public DCFCalculationExecutionStrategy(IMediator mediator)
        {
            _mediator = mediator;
        }*/
        public DCFCalculationExecutionStrategy(IMediator mediator, string ticker)
        {
            _mediator = mediator;
            _ticker = ticker;
        }

        public async Task<MethodResult<ICalculationResult>> ExecuteCalculationStrategy(IScrapeResult scrapeResult, decimal safetyMargin)
        {
            DCFIntrinsicScrapeResult dcfScrapeResult = (DCFIntrinsicScrapeResult)scrapeResult;
            if (dcfScrapeResult.Ticker is null)
                dcfScrapeResult.Ticker = _ticker;
            DCFIntrinsicModelCommand request = new DCFIntrinsicModelCommand(dcfScrapeResult, safetyMargin);

            Task<DCFCalculationResult> result = _mediator.Send(request);

            await result.ConfigureAwait(false);

            return new MethodResult<ICalculationResult>(result.Result);
        }
    }
}
