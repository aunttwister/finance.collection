using FinanceScraper.Common.Init.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finance.Collection.Domain.Common.Propagation;

namespace FinanceScraper.Common.Init.ExecutionStrategy.Factory
{
    public class ScrapeExecutionStrategyFactory
    {
        private readonly IMediator _mediator;
        private readonly string _ticker;
        private readonly List<IScrapeExecutionStrategy> strategies = new List<IScrapeExecutionStrategy>();

        public ScrapeExecutionStrategyFactory(IMediator mediator, string ticker)
        {
            _mediator = mediator;
            _ticker = ticker;
            // Register individual strategies
            strategies.Add(new CurrentPriceScrapeExecutionStrategy(_mediator, _ticker));
            strategies.Add(new GrahamScrapeExecutionStrategy(_mediator, _ticker));
            strategies.Add(new DCFScrapeExecutionStrategy(_mediator, _ticker));
        }
        [HandleMethodExecutionAspect]
        public MethodResult<IScrapeExecutionStrategy> GetScrapeExecutionStrategy(InitScrapeCommand request)
        {
            var selectedStrategies = new List<IScrapeExecutionStrategy>
            {
                strategies.OfType<CurrentPriceScrapeExecutionStrategy>().First() //Mandatory scrape
            };

            if (request.ExecuteGrahamScrape)
            {
                selectedStrategies.Add(strategies.OfType<GrahamScrapeExecutionStrategy>().First());
            }
            if (request.ExecuteDCFScrape)
            {
                selectedStrategies.Add(strategies.OfType<DCFScrapeExecutionStrategy>().First());
            }

            if (!selectedStrategies.Any())
            {
                return new MethodResult<IScrapeExecutionStrategy>(
                    null,
                    new ApplicationException("Unable to resolve scrape execution strategy. Please try again."));
            }

            // Use a composite strategy if more than one strategy is selected
            if (selectedStrategies.Count > 1)
            {
                return new MethodResult<IScrapeExecutionStrategy>(new CompositeScrapeExecutionStrategy(selectedStrategies, _ticker));
            }

            // Only one strategy is selected
            return new MethodResult<IScrapeExecutionStrategy>(selectedStrategies.Single());
        }
    }
}
