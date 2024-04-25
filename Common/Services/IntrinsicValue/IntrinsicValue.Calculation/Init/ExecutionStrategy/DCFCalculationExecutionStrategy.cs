using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.FinanceScraper.Constants;
using Finance.Collection.Domain.Common.DataSets;
using Finance.Collection.Domain.FinanceScraper.Results;
using Finance.Collection.Domain.IntrinsicValue.Calculation.Results;
using MediatR;

namespace IntrinsicValue.Calculation.Init.ExecutionStrategy
{
    public class DCFCalculationExecutionStrategy : ICalculationExecutionStrategy
    {
        private readonly IMediator _mediator;
        private readonly IScrapeResult _scrapeResult;
        public DCFCalculationExecutionStrategy(IMediator mediator, IScrapeResult scrapeResult)
        {
            _mediator = mediator;
            _scrapeResult = scrapeResult;
        }

        public Task<MethodResult<ICalculationResult>> ExecuteCalculationStrategy()
        {
            throw new NotImplementedException();
        }

        /*public async Task<MethodResult<IScrapeResult>> ExecuteScrapeStrategy()
        {
            StockAnalysisCashFlowScraperCommand cashFlowRequest = new StockAnalysisCashFlowScraperCommand(_ticker, UrlPathConstants.StockAnalysisCashFlowScraperPath);
            Task<CashFlowDataSet> cashFlowTask = _mediator.Send(cashFlowRequest);

            StockAnalysisBalanceSheetScraperCommand balanceSheetRequest = new StockAnalysisBalanceSheetScraperCommand(_ticker, UrlPathConstants.StockAnalysisBalanceSpreadSheetPath);
            Task<BalanceSheetDataSet> balanceSheetTask = _mediator.Send(balanceSheetRequest);

            StockAnalysisStatisticsScraperCommand statisticsRequest = new StockAnalysisStatisticsScraperCommand(_ticker, UrlPathConstants.StockAnalysisStatisticsSheetPath);
            Task<StatisticsDataSet> statisticsTask = _mediator.Send(statisticsRequest);

            await Task.WhenAll(cashFlowTask, balanceSheetTask, statisticsTask);

            DCFIntrinsicScrapeResult dcfScrapeResult = new DCFIntrinsicScrapeResult()
            {
                CashFlow = cashFlowTask.Result,
                BalanceSheet = balanceSheetTask.Result,
                Statistics = statisticsTask.Result
            };

            MethodResult<IScrapeResult> result = new MethodResult<IScrapeResult>();

            result.AssignData(dcfScrapeResult);
            return result;
        }*/
    }
}
