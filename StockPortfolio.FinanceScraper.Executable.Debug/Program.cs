using Calculation.Intrinsic;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StockPortfolio.FinanceScraper.Common.Constants;
using StockPortfolio.FinanceScraper.Common.DataSets;
using StockPortfolio.FinanceScraper.Common.Extensions;
using StockPortfolio.FinanceScraper.MacroTrends.CashFlow.Commands;
using StockPortfolio.FinanceScraper.StockAnalysis.BalanceSheetScraper.Commands;
using StockPortfolio.FinanceScraper.StockAnalysis.StatisticsScraper.Commands;

namespace StockPortfolio.FinanceScraper.Executable.Debug
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            IServiceProvider serviceProvider = CreateHostBuilder().Build().Services;
            IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();

            string ticker = "CMG";

            TickerDataSet tickerDataSet = new TickerDataSet();

            BalanceSheetScraperCommand balanceSheetRequest = new BalanceSheetScraperCommand(ticker, UrlPathConstants.StockAnalysisBalanceSpreadSheetPath);
            Task<BalanceSheetDataSet> taskBalanceSheet = _mediator.Send(balanceSheetRequest);

            StatisticsScraperCommand statisticsScraperCommand = new StatisticsScraperCommand(ticker, UrlPathConstants.StockAnalysisStatisticsSheetPath);
            Task<StatisticsDataSet> taskStatisticsDataSet = _mediator.Send(statisticsScraperCommand);

            await Task.WhenAll(taskBalanceSheet, taskStatisticsDataSet);

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
    }
}