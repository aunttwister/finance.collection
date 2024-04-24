using FinanceScraper.Common.Init.Commands;
using FinanceScraper.Common.Propagation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScraper.Common.Init.ExecutionStrategy.Factory
{
    public class ScrapeStrategyFactory
    {
        private readonly IMediator _mediator;
        private readonly string _ticker;
        private readonly List<IExecutionScrapeStrategy> strategies = new List<IExecutionScrapeStrategy>();

        public ScrapeStrategyFactory(IMediator mediator, string ticker)
        {
            _mediator = mediator;
            _ticker = ticker;
            // Register individual strategies
            strategies.Add(new GrahamExecutionScrapeStrategy(_mediator, _ticker));
            strategies.Add(new DCFExecutionScrapeStrategy(_mediator, _ticker));
        }

        public MethodResult<IExecutionScrapeStrategy> GetStrategy(InitCommand request)
        {
            var selectedStrategies = new List<IExecutionScrapeStrategy>();

            if (request.ExecuteGrahamScrape)
            {
                selectedStrategies.Add(strategies.OfType<GrahamExecutionScrapeStrategy>().First());
            }
            if (request.ExecuteDCFScrape)
            {
                selectedStrategies.Add(strategies.OfType<DCFExecutionScrapeStrategy>().First());
            }

            if (!selectedStrategies.Any())
            {
                return new MethodResult<IExecutionScrapeStrategy>(
                    null,
                    new ApplicationException("Unable to resolve execution strategy. Please try again."));
            }

            // Use a composite strategy if more than one strategy is selected
            if (selectedStrategies.Count > 1)
            {
                return new MethodResult<IExecutionScrapeStrategy>(new CompositeExecutionScrapeStrategy(selectedStrategies, _ticker));
            }

            // Only one strategy is selected
            return new MethodResult<IExecutionScrapeStrategy>(selectedStrategies.Single());
        }
    }
}
