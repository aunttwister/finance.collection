using IntrinsicValue.Calculation;
using IntrinsicValue.Calculation.DataSets.GrahamIntrinsicModel;
using IntrinsicValue.Calculation.GrahamIntrinsicModel.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FinanceScraper.Common.Constants;
using FinanceScraper.Common.DataSets;
using FinanceScraper.Common.Extensions;
using StockPortfolio.FinanceScraper.Executable;
using FinanceScraper.YahooFinance.AnalysisScraper.Commands;
using FinanceScraper.YahooFinance.CashFlowScraper.Commands;
using FinanceScraper.YahooFinance.SummaryScraper.Commands;
using FinanceScraper.YCharts.TripleABondYieldScraper.Commands;

namespace Scraper.YahooFinanceScraper
{
    public class Program
    {
        static async Task Main()
        {
            IServiceProvider serviceProvider = CreateHostBuilder().Build().Services;
            IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();

            string ticker = "DIS";

            var startTime = DateTime.Now;

            ScraperDataSetStruct dataSetStruct = await RunScrapersAsync(ticker, _mediator);

            //Calculate
            GrahamIntrinsicModelCommand grahamIntrinsicModelRequest;
            GrahamIntrinsicModelDataSet grahamIntrinsicModelDataSet;
            decimal buyPrice;

            grahamIntrinsicModelRequest = new GrahamIntrinsicModelCommand(ticker, dataSetStruct.TickerDataSet.CurrentPrice)
            {
                Eps = dataSetStruct.TickerDataSet.Summary.Eps,
                FiveYearGrowth = dataSetStruct.TickerDataSet.Analysis.FiveYearGrowth,
                AverageBondYield = dataSetStruct.TripleABondsDataSet.HistoricalAverageTripleABond,
                CurrentBondYield = dataSetStruct.TripleABondsDataSet.CurrentTripleABond
            };
            grahamIntrinsicModelDataSet = await _mediator.Send(grahamIntrinsicModelRequest);

            buyPrice = Math.Round(grahamIntrinsicModelDataSet.IntrinsicValue.Value * 0.65m, 2);

            Console.WriteLine(ticker);
            Console.WriteLine("Intrisic value:" + grahamIntrinsicModelDataSet.IntrinsicValue.Value);
            Console.WriteLine("Buy price:" + buyPrice);
            Console.WriteLine("Current price:" + dataSetStruct.TickerDataSet.Summary.CurrentPrice);

            decimal priceDiff = buyPrice > 0 ? Math.Round((buyPrice - dataSetStruct.TickerDataSet.Summary.CurrentPrice) / buyPrice * 100, 2) : Math.Round((buyPrice - dataSetStruct.TickerDataSet.Summary.CurrentPrice) / buyPrice * -100, 2);

            Console.WriteLine("Price difference:" + priceDiff + "%");
            Console.WriteLine("\nExpected 5 year growth (Average):" + grahamIntrinsicModelDataSet.FiveYearGrowth + "%");
            Console.WriteLine("EPS:" + grahamIntrinsicModelDataSet.Eps);

            Console.WriteLine("run time: " + (DateTime.Now - startTime));

            Console.ReadLine();
        }
        public async static Task<ScraperDataSetStruct> RunScrapersAsync(string ticker, IMediator _mediator)
        {
            /*CurrentPriceScraperCommand currentPriceRequest = new CurrentPriceScraperCommand(ticker);
            tickerDataSet.CurrentPrice = await _mediator.Send(currentPriceRequest);*/

            SummaryScraperCommand summaryRequest = new SummaryScraperCommand(ticker, UrlPathConstants.YahooFinanceSummaryScraperPath);
            Task<SummaryDataSet> summaryTask = _mediator.Send(summaryRequest);

            AnalysisScraperCommand analysisRequest = new AnalysisScraperCommand(ticker, UrlPathConstants.YahooFinanceAnalysisScraperPath);
            Task<AnalysisDataSet> analysisTask = _mediator.Send(analysisRequest);

            //MacroTrendsCashFlowScraperCommand macroTrendsCashFlowRequest = new MacroTrendsCashFlowScraperCommand(ticker, UrlPathConstants.MacroTrendsCashFlowScraperPath);
            //tickerDataSet.CashFlow = await _mediator.Send(macroTrendsCashFlowRequest);

            YahooFinanceCashFlowScraperCommand yahooCashFlowRequest = new YahooFinanceCashFlowScraperCommand(ticker, UrlPathConstants.YahooFinanceCashFlowScraperPath);
            Task<CashFlowDataSet> cashFlowTask = _mediator.Send(yahooCashFlowRequest);

            TripleABondYieldScraperCommand tripleABondYieldRequest = new TripleABondYieldScraperCommand();
            Task<TripleABondsDataSet> tripleABondYieldTask = _mediator.Send(tripleABondYieldRequest);

            await Task.WhenAll(summaryTask, analysisTask, cashFlowTask, tripleABondYieldTask);

            TickerDataSet tickerDataSet = new TickerDataSet()
            {
                Summary = summaryTask.Result,
                Analysis = analysisTask.Result,
                CashFlow = cashFlowTask.Result
            };

            TripleABondsDataSet tripleABondsDataSet = tripleABondYieldTask.Result;

            return new ScraperDataSetStruct()
            {
                TickerDataSet = tickerDataSet,
                TripleABondsDataSet = tripleABondsDataSet
            };

        }
        public static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                       .ConfigureServices((hostContext, services) =>
                       {
                           services.RegisterMediatR();
                           services.RegisterFinanceScraperServices();
                           services.RegisterIntrinsicCalculatorServices();
                       });
        }
    }
}