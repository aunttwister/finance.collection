using Finance.Collection.Domain.FinanceScraper.Constants;
using Finance.Collection.Domain.FinanceScraper.Results;
using FinanceScraper.Common.Init.Commands;
using FinanceScraper.StockAnalysis.BalanceSheetScraper.Commands;
using FinanceScraper.StockAnalysis.CashFlowScraper.Commands;
using FinanceScraper.StockAnalysis.StatisticsScraper.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.FinanceScraper.DataSets;

namespace FinanceScraper.Common.Init.ExecutionStrategy
{
    public class DCFScrapeExecutionStrategy : IScrapeExecutionStrategy
    {
        private readonly IMediator _mediator;
        private readonly string _ticker;
        public DCFScrapeExecutionStrategy(IMediator mediator, string ticker)
        {
            _mediator = mediator;
            _ticker = ticker;
        }
        public async Task<MethodResult<IScrapeResult>> ExecuteScrapeStrategy()
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
                Ticker = _ticker,
                CashFlow = cashFlowTask.Result,
                BalanceSheet = balanceSheetTask.Result,
                Statistics = statisticsTask.Result
            };

            MethodResult<IScrapeResult> result = new MethodResult<IScrapeResult>();

            result.AssignData(dcfScrapeResult);
            return result;
        }
    }
}
