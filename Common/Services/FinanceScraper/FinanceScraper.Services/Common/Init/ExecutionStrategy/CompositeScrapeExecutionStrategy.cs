using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.FinanceScraper.Results;
using FinanceScraper.Common.NodeResolver;

namespace FinanceScraper.Common.Init.ExecutionStrategy
{
    public class CompositeScrapeExecutionStrategy : IScrapeExecutionStrategy
    {
        private readonly List<IScrapeExecutionStrategy> _strategies;
        private readonly string _ticker;

        public CompositeScrapeExecutionStrategy(IEnumerable<IScrapeExecutionStrategy> strategies, string ticker)
        {
            _strategies = strategies.ToList();
            _ticker = ticker;
        }

        public async Task<MethodResult<IScrapeResult>> ExecuteScrapeStrategy()
        {
            var tasks = _strategies.Select(strategy => strategy.ExecuteScrapeStrategy()).ToList();
            var results = await Task.WhenAll(tasks).ConfigureAwait(false);

            // Combine results, or return a new result that indicates the completion of all tasks.
            CombinedScrapeResult combinedResult = new CombinedScrapeResult();
            combinedResult.Ticker = _ticker;
            foreach (var result in results)
            {
                combinedResult.AddResult(result.Data);
            }

            MethodResult<IScrapeResult> finalResult = new MethodResult<IScrapeResult>();
            finalResult.AssignData(combinedResult);

            return finalResult;
        }
    }
}
