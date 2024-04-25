using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.FinanceScraper.Results;
using FinanceScraper.Common.Init.ExecutionStrategy;
using IntrinsicValue.Calculation.Init.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntrinsicValue.Calculation.Init.ExecutionStrategy.Factory
{
    public class CalculationExecutionStrategyFactory
    {
        private readonly IMediator _mediator;
        private readonly List<ICalculationExecutionStrategy> strategies = new List<ICalculationExecutionStrategy>();

        public CalculationExecutionStrategyFactory(IMediator mediator, IScrapeResult scrapeResult)
        {
            _mediator = mediator;
            // Register individual strategies
            strategies.Add(new GrahamCalculationExecutionStrategy(_mediator, scrapeResult));
            strategies.Add(new DCFCalculationExecutionStrategy(_mediator, scrapeResult));
        }

        //[HandleMethodExecutionAspect]
        public MethodResult<ICalculationExecutionStrategy> GetCalculationExecutionStrategy(InitCommand request)
        {
            var selectedStrategies = new List<ICalculationExecutionStrategy>();

            if (request.ScrapeResult.GetType() == typeof(GrahamIntrinsicScrapeResult))
            {
                selectedStrategies.Add(strategies.OfType<GrahamCalculationExecutionStrategy>().First());
            }
            if (request.ScrapeResult.GetType() == typeof(DCFIntrinsicScrapeResult))
            {
                selectedStrategies.Add(strategies.OfType<DCFCalculationExecutionStrategy>().First());
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
                return new MethodResult<ICalculationExecutionStrategy>(new CompositeCalculationExecutionStrategy(selectedStrategies));
            }

            // Only one strategy is selected
            return new MethodResult<ICalculationExecutionStrategy>(selectedStrategies.Single());
        }
    }
}
