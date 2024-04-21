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
using FinanceScraper.StockAnalysis.CashFlowScraper.Commands;
using FinanceScraper.StockAnalysis.BalanceSheetScraper.Commands;
using FinanceScraper.StockAnalysis.StatisticsScraper.Commands;
using FinanceScraper.Common.CustomDataType;

namespace Scraper.YahooFinanceScraper
{
    public class Program
    {
        static async Task Main()
        {
            IServiceProvider serviceProvider = CreateHostBuilder().Build().Services;
            IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();

            string ticker = "IBTA";

            var startTime = DateTime.Now;

            ScraperDataSetStruct dataSetStruct = await RunScrapersAsync(ticker, _mediator).ConfigureAwait(false);

            //Calculate
            GrahamIntrinsicModelCommand grahamIntrinsicModelRequest;
            GrahamIntrinsicModelDataSet grahamIntrinsicModelDataSet;
            decimal buyPrice;

            grahamIntrinsicModelRequest = new GrahamIntrinsicModelCommand(ticker, dataSetStruct.TickerDataSet.Summary.CurrentPrice.Data)
            {
                Eps = dataSetStruct.TickerDataSet.Summary.Eps.Data,
                FiveYearGrowth = dataSetStruct.TickerDataSet.Analysis.FiveYearGrowth.Data,
                AverageBondYield = dataSetStruct.TripleABondsDataSet.HistoricalAverageTripleABond.Data,
                CurrentBondYield = dataSetStruct.TripleABondsDataSet.CurrentTripleABond.Data,
                CurrentPrice = dataSetStruct.TickerDataSet.Summary.CurrentPrice.Data
            };
            grahamIntrinsicModelDataSet = await _mediator.Send(grahamIntrinsicModelRequest);

            buyPrice = Math.Round(grahamIntrinsicModelDataSet.IntrinsicValue.Value * 0.65m, 2);

            Console.WriteLine(ticker);
            Console.WriteLine("Intrisic value:" + grahamIntrinsicModelDataSet.IntrinsicValue.Value);
            Console.WriteLine("Buy price:" + buyPrice);
            Console.WriteLine("Current price:" + grahamIntrinsicModelDataSet.CurrentPrice);

            //decimal priceDiff = buyPrice > 0 ? Math.Round((buyPrice - dataSetStruct.TickerDataSet.Summary.CurrentPrice.Data) / buyPrice * 100, 2) : Math.Round((buyPrice - dataSetStruct.TickerDataSet.Summary.CurrentPrice.Data) / buyPrice * -100, 2);

            //Console.WriteLine("Price difference:" + priceDiff + "%");
            Console.WriteLine("\nExpected 5 year growth (Average):" + grahamIntrinsicModelDataSet.FiveYearGrowth + "%");
            Console.WriteLine("EPS:" + grahamIntrinsicModelDataSet.Eps);
            Console.WriteLine($"{dataSetStruct.TickerDataSet.Summary.Eps.Exception}");
            Console.WriteLine($"{dataSetStruct.TickerDataSet.Summary.CurrentPrice.Exception}");

            //Historical Cash Flow
            Console.WriteLine("\nHistorical Cash Flow");

            DisplayDictionaryWithExceptionsResults(dataSetStruct.TickerDataSet.CashFlow.HistoricalYearCashFlows);

            //Balance Sheet
            Console.WriteLine("\nBalance Sheet");
            Console.WriteLine("\nHistorical Total Debt");
            DisplayDictionaryWithExceptionsResults(dataSetStruct.TickerDataSet.BalanceSheet.HistoricalTotalDebt);
            Console.WriteLine("\nHistorical Cash & Cash Equivalents");
            DisplayDictionaryWithExceptionsResults(dataSetStruct.TickerDataSet.BalanceSheet.HistoricalCashEquivalents);

            Console.WriteLine($"TTM Cash and Cash Equivalents: {dataSetStruct.TickerDataSet.BalanceSheet.TTMCashEquivalents}");
            Console.WriteLine($"TTM Total Debt: {dataSetStruct.TickerDataSet.BalanceSheet.TTMTotalDebt}");

            //Statistics
            Console.WriteLine("\nStatistics");
            Console.WriteLine($"Shares outstanding: {dataSetStruct.TickerDataSet.Statistics.SharesOutstanding.Data}");

            //Performance
            Console.WriteLine("\nPerformance");
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

            StockAnalysisCashFlowScraperCommand cashFlowRequest = new StockAnalysisCashFlowScraperCommand(ticker, UrlPathConstants.StockAnalysisCashFlowScraperPath);
            Task<CashFlowDataSet> cashFlowTask = _mediator.Send(cashFlowRequest);

            StockAnalysisBalanceSheetScraperCommand balanceSheetRequest = new StockAnalysisBalanceSheetScraperCommand(ticker, UrlPathConstants.StockAnalysisBalanceSpreadSheetPath);
            Task<BalanceSheetDataSet> balanceSheerTask = _mediator.Send(balanceSheetRequest);

            StockAnalysisStatisticsScraperCommand statisticsRequest = new StockAnalysisStatisticsScraperCommand(ticker, UrlPathConstants.StockAnalysisStatisticsSheetPath);
            Task<StatisticsDataSet> statisticsTask = _mediator.Send(statisticsRequest);

            TripleABondYieldScraperCommand tripleABondYieldRequest = new TripleABondYieldScraperCommand();
            Task<TripleABondsDataSet> tripleABondYieldTask = _mediator.Send(tripleABondYieldRequest);

            await Task.WhenAll(summaryTask, analysisTask, cashFlowTask, tripleABondYieldTask);

            TickerDataSet tickerDataSet = new TickerDataSet()
            {
                Summary = summaryTask.Result,
                Analysis = analysisTask.Result, 
                CashFlow = cashFlowTask.Result,
                BalanceSheet = balanceSheerTask.Result,
                Statistics = statisticsTask.Result
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

        private static void DisplayDictionaryWithExceptionsResults(DictionaryWithKeyValuePairExceptions<string, decimal> dictionary)
        {
            if (dictionary == null)
                return;
            else
            {
                if (dictionary.Dictionary is not null)
                {
                    foreach (var item in dictionary.Dictionary)
                    {
                        Console.WriteLine($"{item.Key} - ${item.Value}");
                    }
                }
                else
                {
                    Console.WriteLine($"\nExceptions occurred. {dictionary.KeyValuePairExceptions.Key}");
                    Console.WriteLine($"Exceptions occurred. {dictionary.KeyValuePairExceptions.Value}\n");
                }
            }
        }
    }
}