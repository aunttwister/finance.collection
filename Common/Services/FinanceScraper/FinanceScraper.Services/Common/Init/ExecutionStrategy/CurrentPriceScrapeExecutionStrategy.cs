using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.FinanceScraper.Constants;
using Finance.Collection.Domain.FinanceScraper.DataSets;
using Finance.Collection.Domain.FinanceScraper.Results;
using FinanceScraper.StockAnalysis.BalanceSheetScraper.Commands;
using FinanceScraper.StockAnalysis.CashFlowScraper.Commands;
using FinanceScraper.StockAnalysis.StatisticsScraper.Commands;
using FinanceScraper.YahooFinance.CurrentPriceScraper.Commands;
using FinanceScraper.YahooFinance.SummaryScraper.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScraper.Common.Init.ExecutionStrategy
{

    public class CurrentPriceScrapeExecutionStrategy : IScrapeExecutionStrategy
    {
        private readonly IMediator _mediator;
        private readonly string _ticker;
        public CurrentPriceScrapeExecutionStrategy(IMediator mediator, string ticker)
        {
            _mediator = mediator;
            _ticker = ticker;
        }
        public async Task<MethodResult<IScrapeResult>> ExecuteScrapeStrategy()
        {
            YahooFinanceCurrentPriceScraperCommand currentPriceRequest = new YahooFinanceCurrentPriceScraperCommand(_ticker, UrlPathConstants.YahooFinanceSummaryScraperPath);
            Task<CurrentPriceDataSet> currentPriceTask = _mediator.Send(currentPriceRequest);

            await currentPriceTask;

            CurrentPriceScrapeResult currentPriceScrapeResult = new CurrentPriceScrapeResult()
            {
                CurrentPrice = currentPriceTask.Result
            };

            MethodResult<IScrapeResult> result = new MethodResult<IScrapeResult>();

            result.AssignData(currentPriceScrapeResult);
            return result;
        }
    }
}
