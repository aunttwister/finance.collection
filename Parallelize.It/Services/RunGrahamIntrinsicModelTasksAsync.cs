using MediatR;
using StockPortfolio.FinanceScraper.Common.Constants;
using StockPortfolio.FinanceScraper.Common.DataSets;
using StockPortfolio.FinanceScraper.YahooFinance.AnalysisScraper.Commands;
using StockPortfolio.FinanceScraper.YahooFinance.CashFlowScraper.Commands;
using StockPortfolio.FinanceScraper.YahooFinance.SummaryScraper.Commands;
using StockPortfolio.FinanceScraper.YCharts.TripleABondYieldScraper.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculation.Intrinsic.DataSets.GrahamIntrinsicModel;
using Calculation.Intrinsic.GrahamIntrinsicModel.Commands;

namespace Parallelize.It.Services
{
    public class RunGrahamIntrinsicModelTasksAsync : IRunTasksAsyncService<GrahamIntrinsicModelCommand>
    {
        private readonly IMediator _mediator;
        public RunGrahamIntrinsicModelTasksAsync(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<GrahamIntrinsicModelCommand> RunScrapersAsync(GrahamIntrinsicModelCommand request)
        {

            SummaryScraperCommand summaryRequest = new SummaryScraperCommand(request.Ticker, UrlPathConstants.YahooFinanceSummaryScraperPath);
            Task<SummaryDataSet> summaryTask = _mediator.Send(summaryRequest);

            AnalysisScraperCommand analysisRequest = new AnalysisScraperCommand(request.Ticker, UrlPathConstants.YahooFinanceAnalysisScraperPath);
            Task<AnalysisDataSet> analysisTask = _mediator.Send(analysisRequest);

            TripleABondYieldScraperCommand tripleABondYieldRequest = new TripleABondYieldScraperCommand();
            Task<TripleABondsDataSet> tripleABondYieldTask = _mediator.Send(tripleABondYieldRequest);

            await Task.WhenAll(summaryTask, analysisTask, tripleABondYieldTask);

            request.Eps = summaryTask.Result.Eps;
            request.FiveYearGrowth = analysisTask.Result.FiveYearGrowth;
            request.AverageBondYield = tripleABondYieldTask.Result.HistoricalAverageTripleABond;
            request.CurrentBondYield = tripleABondYieldTask.Result.CurrentTripleABond;

            return request;
        }
    }
}
