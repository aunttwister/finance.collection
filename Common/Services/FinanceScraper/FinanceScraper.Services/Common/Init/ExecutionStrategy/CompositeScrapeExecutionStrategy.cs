using Finance.Collection.Domain.FinanceScraper.Constants;
using Finance.Collection.Domain.FinanceScraper.DataSets;
using Finance.Collection.Domain.FinanceScraper.Results;
using FinanceScraper.Common.Init.Commands;
using Finance.Collection.Domain.FinanceScraper.Propagation;
using FinanceScraper.StockAnalysis.BalanceSheetScraper.Commands;
using FinanceScraper.StockAnalysis.CashFlowScraper.Commands;
using FinanceScraper.StockAnalysis.StatisticsScraper.Commands;
using FinanceScraper.YahooFinance.AnalysisScraper.Commands;
using FinanceScraper.YahooFinance.SummaryScraper.Commands;
using FinanceScraper.YCharts.TripleABondYieldScraper.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScraper.Common.Init.ExecutionStrategy
{
    public class CompositeScrapeExecutionStrategy : IScrapeExecutionStrategy
    {
        private readonly List<IScrapeExecutionStrategy> _strategies;
        private readonly string _ticker;

        public CompositeScrapeExecutionStrategy(IEnumerable<IScrapeExecutionStrategy> strategies, string ticker)
        {
            _ticker = ticker;
            _strategies = strategies.ToList();
        }

        public async Task<MethodResult<IScrapeResult>> ExecuteScrapeStrategy()
        {
            var tasks = _strategies.Select(strategy => strategy.ExecuteScrapeStrategy()).ToList();
            var results = await Task.WhenAll(tasks).ConfigureAwait(false);

            // Combine results, or return a new result that indicates the completion of all tasks.
            CombinedScrapeResult combinedResult = new CombinedScrapeResult();
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
