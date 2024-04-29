using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.FinanceScraper.Results;
using Finance.Collection.Domain.IntrinsicValue.Calculation.Results;
using IntrinsicValue.Calculation.Init.Commands;
using IntrinsicValue.Calculation.Init.ExecutionStrategy.Factory;
using MediatR;

namespace IntrinsicValue.Calculation.Init.ExecutionStrategy
{
    public class CompositeCalculationExecutionStrategy : ICalculationExecutionStrategy
    {
        private ICalculationExecutionStrategy _executionStrategy;
        private readonly List<ICalculationExecutionStrategy> _strategies;
        private readonly IMediator _mediator;

        public CompositeCalculationExecutionStrategy(IEnumerable<ICalculationExecutionStrategy> strategies, IMediator mediator)
        {
            _strategies = strategies.ToList();
            _mediator = mediator;
        }

        public async Task<MethodResult<ICalculationResult>> ExecuteCalculationStrategy(IScrapeResult scrapeResult, decimal safetyMargin)
        {
            CombinedScrapeResult combinedScrape = (CombinedScrapeResult)scrapeResult;
            Dictionary<Type, IScrapeResult> scrapeResults = combinedScrape.GetAllResults();

            List<Task<MethodResult<ICalculationResult>>> tasks = new List<Task<MethodResult<ICalculationResult>>>();
            foreach (IScrapeResult item in scrapeResults.Values)
            {
                MethodResult<ICalculationExecutionStrategy> strategyResolveResult = SwitchStrategy(item, _mediator, combinedScrape.Ticker);
                if (!strategyResolveResult.IsSuccessful)
                {
                    return new MethodResult<ICalculationResult>(
                        null,
                        strategyResolveResult.Exception);
                }
                _executionStrategy = strategyResolveResult.Data;

                tasks.Add(_executionStrategy.ExecuteCalculationStrategy(item, safetyMargin));
            }

            var results = await Task.WhenAll(tasks).ConfigureAwait(false);

            // Combine results, or return a new result that indicates the completion of all tasks.
            CombinedCalculationResult combinedResult = new CombinedCalculationResult();
            foreach (var result in results)
            {
                combinedResult.AddResult(result.Data);
            }

            MethodResult<ICalculationResult> finalResult = new MethodResult<ICalculationResult>();
            finalResult.AssignData(combinedResult);


            return finalResult;
        }

        private MethodResult<ICalculationExecutionStrategy> SwitchStrategy(IScrapeResult scrapeResult, IMediator _mediator, string ticker)
        {
            var factory = new CalculationExecutionStrategyFactory(_mediator, ticker);
            return factory.GetCalculationExecutionStrategy(scrapeResult);
        }
    }
}
