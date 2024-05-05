using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.FinanceScraper.Constants;
using Finance.Collection.Domain.FinanceScraper.Results;
using FinanceScraper.Common.Init.ExecutionStrategy;
using FinanceScraper.Common.Init.ExecutionStrategy.Factory;
using FinanceScraper.Common.NodeResolver;
using FinanceScraper.Common.NodeResolver.Factory;
using FinanceScraper.Common.NodeResolver.ServiceProvider;
using HtmlAgilityPack;
using MediatR;

namespace FinanceScraper.Common.Init.Commands
{
    public class InitScrapeCommandHandler : IRequestHandler<InitScrapeCommand, MethodResult<IScrapeResult>>
    {
        private IScrapeExecutionStrategy _executionStrategy;
        private readonly IMediator _mediator;
        private readonly INodeResolverStrategyProvider _nodeResolverStrategyProvider;
        public InitScrapeCommandHandler(IMediator mediator, INodeResolverStrategyProvider nodeResolverStrategyProvider)
        {
            _mediator = mediator;
            _nodeResolverStrategyProvider = nodeResolverStrategyProvider;
        }
        public async Task<MethodResult<IScrapeResult>> Handle(InitScrapeCommand request, CancellationToken cancellationToken)
        {
            INodeResolverStrategy nodeResolverStrategy = SwitchNodeResolverStrategy(request.UseHtmlContent);
            _nodeResolverStrategyProvider.SetCurrentStrategy(nodeResolverStrategy);

            MethodResult<string> validationResult = await ValidateTicker(request).ConfigureAwait(false);
            if (!validationResult.IsSuccessful)
            {
                ApplicationException exception = new ApplicationException(validationResult.Exception.Message);
                return new MethodResult<IScrapeResult>(null, exception);
            }

            MethodResult<IScrapeExecutionStrategy> strategyResolveResult = SwitchExecutionStrategy(request, _mediator);
            if (!strategyResolveResult.IsSuccessful)
            {
                return new MethodResult<IScrapeResult>(
                    null,
                    strategyResolveResult.Exception);
            }
            _executionStrategy = strategyResolveResult.Data;

            return await _executionStrategy.ExecuteScrapeStrategy();
        }
        public INodeResolverStrategy SwitchNodeResolverStrategy(bool useHtmlContent)
        {
            NodeResolverFactory factory = new NodeResolverFactory();
            return factory.GetResolverStrategy(useHtmlContent);
        }

        private MethodResult<IScrapeExecutionStrategy> SwitchExecutionStrategy(InitScrapeCommand request, IMediator _mediator)
        {
            var factory = new ScrapeExecutionStrategyFactory(_mediator, request.Ticker);
            return factory.GetScrapeExecutionStrategy(request);
        }
        [HandleMethodExecutionAspect]
        private async Task<MethodResult<string>> ValidateTicker(InitScrapeCommand request)
        {
            // Simplify by directly handling the no-operation scenario
            if (!request.ExecuteGrahamScrape && !request.ExecuteDCFScrape)
            {
                return new MethodResult<string>(
                    request.Ticker,
                    new ApplicationException("Invalid parameter combination. Please try again."));
            }

            // Initialize tasks for each possible operation
            var tasks = new List<Task<MethodResult<string>>>();

            if (request.ExecuteGrahamScrape)
            {
                tasks.Add(ValidateTickerStockAnalysis(request.Ticker));
            }
            if (request.ExecuteDCFScrape)
            {
                tasks.Add(ValidateTickerYahooFinance(request.Ticker));
            }

            // Await all initiated tasks
            await Task.WhenAll(tasks).ConfigureAwait(false);

            // Check results and decide the outcome
            bool allSuccessful = tasks.All(task => task.Result.IsSuccessful);
            if (allSuccessful)
            {
                return new MethodResult<string>(request.Ticker);
            }

            // Collect exceptions from tasks that failed
            var exceptions = tasks.Where(task => !task.Result.IsSuccessful)
                                  .Select(task => task.Result.Exception).ToList();

            // If there are multiple exceptions, concatenate their messages
            if (exceptions.Count > 1)
            {
                var combinedException = new ApplicationException(
                    $"Multiple errors occurred: {string.Join(" | ", exceptions.Select(ex => ex.Message))}");
                return new MethodResult<string>(request.Ticker, combinedException);
            }

            // Return the single exception if only one failed
            return new MethodResult<string>(request.Ticker, exceptions.FirstOrDefault());
        }

        private async Task<MethodResult<string>> ValidateTickerStockAnalysis(string ticker)
        {
            INodeResolverStrategy nodeResolverStrategy = _nodeResolverStrategyProvider.GetCurrentStrategy();
            HtmlNode node = await nodeResolverStrategy.ResolveNodeAsync(BaseUrlConstants.StockAnalysis + ticker).ConfigureAwait(false);

            if (node.InnerText.Contains("Not Found - 404"))
            {
                ApplicationException exception = new ApplicationException($"Unable to ValidateTicker {ticker} on Stock Analysis. Please double check and try again.");
                return new MethodResult<string>(ticker, exception);
            }

            return new MethodResult<string>(ticker);
        }
        private async Task<MethodResult<string>> ValidateTickerYahooFinance(string ticker)
        {
            INodeResolverStrategy nodeResolverStrategy = _nodeResolverStrategyProvider.GetCurrentStrategy();
            HtmlNode node = await nodeResolverStrategy.ResolveNodeAsync(BaseUrlConstants.YahooFinance + ticker).ConfigureAwait(false);

            if (node.InnerText.Contains("Symbols similar to"))
            {
                ApplicationException exception = new ApplicationException($"Unable to ValidateTicker {ticker} on Yahoo Finance. Please double check and try again.");
                return new MethodResult<string>(ticker, exception);
            }

            return new MethodResult<string>(ticker);
        }
    }
}
