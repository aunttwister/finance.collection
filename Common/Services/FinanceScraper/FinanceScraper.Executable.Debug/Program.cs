using IntrinsicValue.Calculation;
using IntrinsicValue.Calculation.GrahamIntrinsicModel.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Finance.Collection.Domain.FinanceScraper.Constants;
using FinanceScraper.Common.Extensions;
using FinanceScraper.YahooFinance.AnalysisScraper.Commands;
using FinanceScraper.YahooFinance.SummaryScraper.Commands;
using FinanceScraper.YCharts.TripleABondYieldScraper.Commands;
using FinanceScraper.StockAnalysis.CashFlowScraper.Commands;
using FinanceScraper.StockAnalysis.BalanceSheetScraper.Commands;
using FinanceScraper.StockAnalysis.StatisticsScraper.Commands;
using Finance.Collection.Domain.FinanceScraper.Results;
using IntrinsicValue.Calculation.DCFIntrinsicModel.Commands;
using ConsoleTables;
using IntrinsicValue.Calculation.DataSets.Results;
using Finance.Collection.Domain.FinanceScraper.DataSets;

namespace FinanceScraper.Executable.Debug
{
    public class Program
    {
        static async Task Main()
        {
            IServiceProvider serviceProvider = CreateHostBuilder().Build().Services;
            IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();

            string ticker = "V";
            bool executeDCF = true;

            var startTime = DateTime.Now;

            Task<GrahamIntrinsicScrapeResult> grahamIntrinsicScrapeData = RunGrahamIntrinsicScrapersAsync(ticker, _mediator);

            ScrapeResult scrapeData = new ScrapeResult();
            if (executeDCF)
            {
                Task<DCFIntrinsicScrapeResult> dcfIntrinsicScrapeResult = RunDCFIntrinsicScrapersAsync(ticker, _mediator);
                await Task.WhenAll(grahamIntrinsicScrapeData, dcfIntrinsicScrapeResult).ConfigureAwait(false);

                scrapeData.GrahamIntrinsicScrapeResult = grahamIntrinsicScrapeData.Result;
                scrapeData.DCFIntrinsicScrapeResult = dcfIntrinsicScrapeResult.Result;

                Task<GrahamCalculationResult> grahamCalculationResultData = CalculateGrahamIntrinsicValueModel(_mediator, ticker, startTime, scrapeData);
                Task<DCFCalculationResult> dcfCalculationResultData = CalculateDCFIntrinsicValueModel(_mediator, ticker, startTime, scrapeData);

                await Task.WhenAll(grahamCalculationResultData, dcfCalculationResultData).ConfigureAwait(false);

                DisplayGrahamIntrinsicValueResult(grahamCalculationResultData.Result, startTime);
                DisplayDCFIntrinsicValueResult(dcfCalculationResultData.Result, startTime);
            }
            else
            {
                await grahamIntrinsicScrapeData.ConfigureAwait(false);

                scrapeData.GrahamIntrinsicScrapeResult = grahamIntrinsicScrapeData.Result;

                Task<GrahamCalculationResult> grahamCalculationResultData = CalculateGrahamIntrinsicValueModel(_mediator, ticker, startTime, scrapeData);

                GrahamCalculationResult resultData = await grahamCalculationResultData.ConfigureAwait(false);

                DisplayGrahamIntrinsicValueResult(resultData, startTime);
            }

            Console.ReadLine();
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

        public async static Task<GrahamIntrinsicScrapeResult> RunGrahamIntrinsicScrapersAsync(string ticker, IMediator _mediator)
        {
            SummaryScraperCommand summaryRequest = new SummaryScraperCommand(ticker, UrlPathConstants.YahooFinanceSummaryScraperPath);
            Task<SummaryDataSet> summaryTask = _mediator.Send(summaryRequest);

            AnalysisScraperCommand analysisRequest = new AnalysisScraperCommand(ticker, UrlPathConstants.YahooFinanceAnalysisScraperPath);
            Task<AnalysisDataSet> analysisTask = _mediator.Send(analysisRequest);

            TripleABondYieldScraperCommand tripleABondYieldRequest = new TripleABondYieldScraperCommand();
            Task<TripleABondsDataSet> tripleABondYieldTask = _mediator.Send(tripleABondYieldRequest);

            await Task.WhenAll(summaryTask, analysisTask, tripleABondYieldTask);

            return new GrahamIntrinsicScrapeResult()
            {
                Summary = summaryTask.Result,
                Analysis = analysisTask.Result,
                TripleABonds = tripleABondYieldTask.Result
            };
        }

        public async static Task<DCFIntrinsicScrapeResult> RunDCFIntrinsicScrapersAsync(string ticker, IMediator _mediator)
        {
            StockAnalysisCashFlowScraperCommand cashFlowRequest = new StockAnalysisCashFlowScraperCommand(ticker, UrlPathConstants.StockAnalysisCashFlowScraperPath);
            Task<CashFlowDataSet> cashFlowTask = _mediator.Send(cashFlowRequest);

            StockAnalysisBalanceSheetScraperCommand balanceSheetRequest = new StockAnalysisBalanceSheetScraperCommand(ticker, UrlPathConstants.StockAnalysisBalanceSpreadSheetPath);
            Task<BalanceSheetDataSet> balanceSheetTask = _mediator.Send(balanceSheetRequest);

            StockAnalysisStatisticsScraperCommand statisticsRequest = new StockAnalysisStatisticsScraperCommand(ticker, UrlPathConstants.StockAnalysisStatisticsSheetPath);
            Task<StatisticsDataSet> statisticsTask = _mediator.Send(statisticsRequest);

            await Task.WhenAll(cashFlowTask, balanceSheetTask, statisticsTask);

            return new DCFIntrinsicScrapeResult()
            {
                CashFlow = cashFlowTask.Result,
                BalanceSheet = balanceSheetTask.Result,
                Statistics = statisticsTask.Result
            };
        }

        private async static Task<GrahamCalculationResult> CalculateGrahamIntrinsicValueModel(IMediator _mediator, string ticker, DateTime startTime, ScrapeResult scrapeData)
        {
            if (scrapeData.GrahamIntrinsicScrapeResult.Summary.Eps.Exception is not null)
                Console.WriteLine($"{scrapeData.GrahamIntrinsicScrapeResult.Summary.Eps.Exception}");
            if (scrapeData.GrahamIntrinsicScrapeResult.Summary.CurrentPrice.Exception is not null)
                Console.WriteLine($"{scrapeData.GrahamIntrinsicScrapeResult.Summary.CurrentPrice.Exception}");

            //Calculate

            GrahamIntrinsicModelCommand grahamIntrinsicModelRequest = new GrahamIntrinsicModelCommand(
                ticker,
                scrapeData.GrahamIntrinsicScrapeResult.Summary.CurrentPrice.Data,
                scrapeData.GrahamIntrinsicScrapeResult.Summary.Eps.Data,
                scrapeData.GrahamIntrinsicScrapeResult.Analysis.FiveYearGrowth.Data,
                scrapeData.GrahamIntrinsicScrapeResult.TripleABonds.HistoricalAverageTripleABond.Data,
                scrapeData.GrahamIntrinsicScrapeResult.TripleABonds.CurrentTripleABond.Data
                );

            return await _mediator.Send(grahamIntrinsicModelRequest);
        }

        private async static Task<DCFCalculationResult> CalculateDCFIntrinsicValueModel(IMediator _mediator, string ticker, DateTime startTime, ScrapeResult scrapeData)
        {
            if (scrapeData.GrahamIntrinsicScrapeResult.Summary.Eps.Exception is not null)
                Console.WriteLine($"{scrapeData.GrahamIntrinsicScrapeResult.Summary.Eps.Exception}");
            if (scrapeData.GrahamIntrinsicScrapeResult.Summary.CurrentPrice.Exception is not null)
                Console.WriteLine($"{scrapeData.GrahamIntrinsicScrapeResult.Summary.CurrentPrice.Exception}");

            //Calculate

            DCFIntrinsicModelCommand dcfIntrinsicModelRequest = new DCFIntrinsicModelCommand(
                ticker,
                scrapeData.GrahamIntrinsicScrapeResult.Summary.CurrentPrice.Data,
                scrapeData.DCFIntrinsicScrapeResult.CashFlow.HistoricalCashFlows.Data,
                scrapeData.DCFIntrinsicScrapeResult.BalanceSheet.TTMCashEquivalents,
                scrapeData.DCFIntrinsicScrapeResult.BalanceSheet.TTMTotalDebt,
                scrapeData.DCFIntrinsicScrapeResult.Statistics.SharesOutstanding.Data,
                scrapeData.DCFIntrinsicScrapeResult.BalanceSheet.HistoricalCashEquivalents.Data,
                scrapeData.DCFIntrinsicScrapeResult.BalanceSheet.HistoricalTotalDebt.Data);

            return await _mediator.Send(dcfIntrinsicModelRequest);
        }

        public static void DisplayDCFIntrinsicValueResult(DCFCalculationResult result, DateTime startTime)
        {
            Console.WriteLine("\nDCF Intrinsic Value Calculation Results\n");
            Console.WriteLine(result.Ticker);
            //Historical Cash Flow
            Console.WriteLine("\nHistorical Cash Flow");

            DisplayConsoleTable(result.HistoricalCashFlow);

            //Balance Sheet
            Console.WriteLine("\nBalance Sheet");
            Console.WriteLine("\nHistorical Total Debt");
            DisplayConsoleTable(result.HistoricalTotalDebt);
            Console.WriteLine("\nHistorical Cash & Cash Equivalents");
            DisplayConsoleTable(result.HistoricalCashAndCashEquivalents);

            Console.WriteLine($"TTM Cash and Cash Equivalents: {result.TTMCashAndCashEquivalents}");
            Console.WriteLine($"TTM Total Debt: {result.TTMTotalDebt}");

            //Statistics
            Console.WriteLine("\nStatistics");
            Console.WriteLine($"Shares outstanding: {result.SharesOutstanding}");

            Console.WriteLine("\nEquity Value:");
            Console.Write($"{result.SumFutureCashFlowValue} + {result.TTMCashAndCashEquivalents} - {result.TTMTotalDebt} = ");
            decimal equityValue = Math.Round(result.SumFutureCashFlowValue + result.TTMCashAndCashEquivalents - result.TTMTotalDebt, 2);
            Console.WriteLine($"{equityValue}");

            Console.WriteLine($"\nDiscounted Cash Flow Intrinsic Value:");
            Console.Write($"{result.EquityValue} / {result.SharesOutstanding} = ");
            Console.WriteLine($"{Math.Round(result.DiscountedCashFlowValue.Value, 2)}");

            //Performance
            Console.WriteLine("\nPerformance");
            Console.WriteLine("run time: " + (DateTime.Now - startTime));
        }

        public static void DisplayGrahamIntrinsicValueResult(GrahamCalculationResult result, DateTime startTime)
        {
            Console.WriteLine("Displaying Graham Intrinsic Value Calculation Result\n");
            Console.WriteLine(result.Ticker);
            Console.WriteLine("\nIntrisic value:" + result.IntrinsicValue.Value);
            Console.WriteLine("Buy price:" + result.BuyPrice);
            Console.WriteLine("Current price:" + result.CurrentPrice);


            Console.WriteLine("Price difference:" + result.PriceDifference + "%");
            Console.WriteLine("\nExpected 5 year growth (Average):" + result.FiveYearGrowth + "%");
            Console.WriteLine("EPS:" + result.Eps);

            //Performance
            Console.WriteLine("\nPerformance");
            Console.WriteLine("run time: " + (DateTime.Now - startTime));
        }

        private static void DisplayDictionary(IDictionary<string, decimal> dictionary)
        {
            foreach (var item in dictionary)
            {
                Console.WriteLine($"{item.Key} - ${item.Value}");
            }
        }

        private static void DisplayConsoleTable<T1, T2>(IEnumerable<T1> rowOne, IEnumerable<T2> rowTwo)
        {
            ConsoleTable table = new ConsoleTable();
            table.AddRow(rowOne);
            table.AddRow(rowTwo);
            Console.WriteLine();
        }
        private static void DisplayConsoleTable<T>(IDictionary<string, T> keyValuePairs)
        {
            string[] columns = keyValuePairs.Keys.ToArray();
            object[] rowParam = keyValuePairs.Values.Cast<object>().ToArray();
            ConsoleTable table = new ConsoleTable(columns);
            table.AddRow(rowParam);
            table.Write(Format.MarkDown);
            Console.WriteLine();
        }
    }
}