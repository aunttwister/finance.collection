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
        private readonly string _ticker;

        public CalculationExecutionStrategyFactory(IMediator mediator, string ticker)
        {
            _mediator = mediator;
            _ticker = ticker;
            // Register individual strategies
            strategies.Add(new GrahamCalculationExecutionStrategy(_mediator, ticker));
            strategies.Add(new DCFCalculationExecutionStrategy(_mediator, ticker));
        }

        //[HandleMethodExecutionAspect]
        public MethodResult<ICalculationExecutionStrategy> GetCalculationExecutionStrategy(IEnumerable<Type> scrapeTypes)
        {
            var selectedStrategies = new List<ICalculationExecutionStrategy>();

            if (scrapeTypes.Contains(typeof(GrahamScrapeResult)))
            {
                selectedStrategies.Add(strategies.OfType<GrahamCalculationExecutionStrategy>().First());
            }
            if (scrapeTypes.Contains(typeof(DCFScrapeResult)))
            {
                selectedStrategies.Add(strategies.OfType<DCFCalculationExecutionStrategy>().First());
            }

            if (!selectedStrategies.Any())
            {
                return new MethodResult<ICalculationExecutionStrategy>(
                    null,
                    new ApplicationException($"Unable to resolve calculation execution strategy for ticker {_ticker}. Please try again."));
            }

            // Use a composite strategy if more than one strategy is selected
            if (selectedStrategies.Count > 1)
            {
                return new MethodResult<ICalculationExecutionStrategy>(new CompositeCalculationExecutionStrategy(selectedStrategies));
            }

            // Only one strategy is selected
            return new MethodResult<ICalculationExecutionStrategy>(selectedStrategies.Single());
        }
    }
}
