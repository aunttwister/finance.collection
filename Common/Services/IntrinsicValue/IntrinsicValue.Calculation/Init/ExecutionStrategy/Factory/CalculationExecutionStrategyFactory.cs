using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.FinanceScraper.Results;
using FinanceScraper.Common.Init.ExecutionStrategy;
using IntrinsicValue.Calculation.Init.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntrinsicValue.Calculation.Init.ExecutionStrategy.Factory
{
    public class CalculationExecutionStrategyFactory
    {
        private readonly IMediator _mediator;
        private readonly List<ICalculationExecutionStrategy> strategies = new List<ICalculationExecutionStrategy>();

        /*public CalculationExecutionStrategyFactory(IMediator mediator)
        {
            _mediator = mediator;
            // Register individual strategies
            strategies.Add(new GrahamCalculationExecutionStrategy(_mediator));
            strategies.Add(new DCFCalculationExecutionStrategy(_mediator));
        }*/
        public CalculationExecutionStrategyFactory(IMediator mediator, string ticker)
        {
            _mediator = mediator;
            // Register individual strategies
            strategies.Add(new GrahamCalculationExecutionStrategy(_mediator, ticker));
            strategies.Add(new DCFCalculationExecutionStrategy(_mediator, ticker));
        }

        //[HandleMethodExecutionAspect]
        public MethodResult<ICalculationExecutionStrategy> GetCalculationExecutionStrategy(IScrapeResult result)
        {
            var selectedStrategies = new List<ICalculationExecutionStrategy>();

            if (result.GetType() == typeof(GrahamIntrinsicScrapeResult))
            {
                selectedStrategies.Add(strategies.OfType<GrahamCalculationExecutionStrategy>().First());
            }
            if (result.GetType() == typeof(DCFIntrinsicScrapeResult))
            {
                selectedStrategies.Add(strategies.OfType<DCFCalculationExecutionStrategy>().First());
            }
            if (result.GetType() == typeof(CombinedScrapeResult))
            {
                CombinedScrapeResult combinedScrapeResult = (CombinedScrapeResult)result;
                var scrapeResults = combinedScrapeResult.GetAllResults();
                foreach (var scrapeResult in scrapeResults)
                {
                    if (scrapeResult.Key == typeof(GrahamIntrinsicScrapeResult))
                    {
                        selectedStrategies.Add(strategies.OfType<GrahamCalculationExecutionStrategy>().First());
                    }
                    if (scrapeResult.Key == typeof(DCFIntrinsicScrapeResult))
                    {
                        selectedStrategies.Add(strategies.OfType<DCFCalculationExecutionStrategy>().First());
                    }
                }
            }

            if (!selectedStrategies.Any())
            {
                return new MethodResult<ICalculationExecutionStrategy>(
                    null,
                    new ApplicationException("Unable to resolve calculation execution strategy. Please try again."));
            }

            // Use a composite strategy if more than one strategy is selected
            if (selectedStrategies.Count > 1)
            {
                return new MethodResult<ICalculationExecutionStrategy>(new CompositeCalculationExecutionStrategy(selectedStrategies, _mediator));
            }

            // Only one strategy is selected
            return new MethodResult<ICalculationExecutionStrategy>(selectedStrategies.Single());
        }
    }
}
