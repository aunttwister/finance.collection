using Finance.Collection.Domain.FinanceScraper.Constants;
using Finance.Collection.Domain.FinanceScraper.DataSets;
using Finance.Collection.Domain.FinanceScraper.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntrinsicValue.Calculation.Init.ExecutionStrategy;
using Finance.Collection.Domain.IntrinsicValue.Calculation.Results;
using Finance.Collection.Domain.Common.Propagation;

namespace FinanceScraper.Common.Init.ExecutionStrategy
{
    public class GrahamCalculationExecutionStrategy : ICalculationExecutionStrategy
    {
        private readonly IMediator _mediator;
        private readonly IScrapeResult _scrapeResult;
        public GrahamCalculationExecutionStrategy(IMediator mediator, IScrapeResult scrapeResult)
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
            SummaryScraperCommand summaryRequest = new SummaryScraperCommand(_ticker, UrlPathConstants.YahooFinanceSummaryScraperPath);
            Task<SummaryDataSet> summaryTask = _mediator.Send(summaryRequest);

            AnalysisScraperCommand analysisRequest = new AnalysisScraperCommand(_ticker, UrlPathConstants.YahooFinanceAnalysisScraperPath);
            Task<AnalysisDataSet> analysisTask = _mediator.Send(analysisRequest);

            TripleABondYieldScraperCommand tripleABondYieldRequest = new TripleABondYieldScraperCommand();
            Task<TripleABondsDataSet> tripleABondYieldTask = _mediator.Send(tripleABondYieldRequest);

            await Task.WhenAll(summaryTask, analysisTask, tripleABondYieldTask);

            GrahamIntrinsicScrapeResult grahamScrapeResult = new GrahamIntrinsicScrapeResult()
            {
                Summary = summaryTask.Result,
                Analysis = analysisTask.Result,
                TripleABonds = tripleABondYieldTask.Result
            };

            MethodResult<IScrapeResult> result = new MethodResult<IScrapeResult>();

            result.AssignData(grahamScrapeResult);
            return result;
        }*/
    }
}
