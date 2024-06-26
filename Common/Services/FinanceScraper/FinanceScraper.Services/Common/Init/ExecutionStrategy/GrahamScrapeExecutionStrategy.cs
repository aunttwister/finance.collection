﻿using Finance.Collection.Domain.FinanceScraper.Constants;
using Finance.Collection.Domain.FinanceScraper.Results;
using FinanceScraper.Common.Init.Commands;
using FinanceScraper.YahooFinance.AnalysisScraper.Commands;
using FinanceScraper.YahooFinance.SummaryScraper.Commands;
using FinanceScraper.YCharts.TripleABondYieldScraper.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.FinanceScraper.DataSets;
using FinanceScraper.Common.NodeResolver;

namespace FinanceScraper.Common.Init.ExecutionStrategy
{
    public class GrahamScrapeExecutionStrategy : IScrapeExecutionStrategy
    {
        private readonly IMediator _mediator;
        private readonly string _ticker;
        public GrahamScrapeExecutionStrategy(IMediator mediator, string ticker)
        {
            _mediator = mediator;
            _ticker = ticker;
        }
        public async Task<MethodResult<IScrapeResult>> ExecuteScrapeStrategy()
        {
            YahooFinanceSummaryScraperCommand summaryRequest = new YahooFinanceSummaryScraperCommand(_ticker, UrlPathConstants.YahooFinanceSummaryScraperPath);
            Task<SummaryDataSet> summaryTask = _mediator.Send(summaryRequest);

            AnalysisScraperCommand analysisRequest = new AnalysisScraperCommand(_ticker, UrlPathConstants.YahooFinanceAnalysisScraperPath);
            Task<AnalysisDataSet> analysisTask = _mediator.Send(analysisRequest);

            TripleABondYieldScraperCommand tripleABondYieldRequest = new TripleABondYieldScraperCommand();
            Task<TripleABondsDataSet> tripleABondYieldTask = _mediator.Send(tripleABondYieldRequest);

            await Task.WhenAll(summaryTask, analysisTask, tripleABondYieldTask);

            GrahamScrapeResult grahamScrapeResult = new GrahamScrapeResult()
            {
                Summary = summaryTask.Result,
                Analysis = analysisTask.Result,
                TripleABonds = tripleABondYieldTask.Result
            };

            MethodResult<IScrapeResult> result = new MethodResult<IScrapeResult>();

            result.AssignData(grahamScrapeResult);
            return result;
        }
    }
}
