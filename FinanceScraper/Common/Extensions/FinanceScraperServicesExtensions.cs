using MediatR;
using Microsoft.Extensions.DependencyInjection;
using StockPortfolio.FinanceScraper.Common.Base;
using StockPortfolio.FinanceScraper.Common.DataSets;
using StockPortfolio.FinanceScraper.Common.Exceptions.ExceptionResolver;
using StockPortfolio.FinanceScraper.MacroTrends.CashFlow;
using StockPortfolio.FinanceScraper.MacroTrends.CashFlow.Commands;
using StockPortfolio.FinanceScraper.StockAnalysis.BalanceSheetScraper;
using StockPortfolio.FinanceScraper.StockAnalysis.BalanceSheetScraper.Commands;
using StockPortfolio.FinanceScraper.StockAnalysis.StatisticsScraper;
using StockPortfolio.FinanceScraper.StockAnalysis.StatisticsScraper.Commands;
using StockPortfolio.FinanceScraper.YahooFinance.AnalysisScraper;
using StockPortfolio.FinanceScraper.YahooFinance.AnalysisScraper.Commands;
using StockPortfolio.FinanceScraper.YahooFinance.CashFlowScraper;
using StockPortfolio.FinanceScraper.YahooFinance.CashFlowScraper.Commands;
using StockPortfolio.FinanceScraper.YahooFinance.SummaryScraper;
using StockPortfolio.FinanceScraper.YahooFinance.SummaryScraper.Commands;
using StockPortfolio.FinanceScraper.YCharts.TripleABondYieldScraper;
using StockPortfolio.FinanceScraper.YCharts.TripleABondYieldScraper.Commands;
using System.Reflection;


namespace StockPortfolio.FinanceScraper.Common.Extensions
{
    public static class FinanceScraperServicesExtensions
    {
        public static void RegisterFinanceScraperServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            services.AddTransient<IExceptionResolverService, ExceptionResolverService>();

            services.AddTransient<IScrapeServiceStrategy<MacroTrendsCashFlowScraperCommand, CashFlowDataSet>, MacroTrendsCashFlowScrapeService>();
            services.AddTransient<IScrapeServiceStrategy<SummaryScraperCommand, SummaryDataSet>, YahooFinanceSummaryScrapeService>();
            services.AddTransient<IScrapeServiceStrategy<AnalysisScraperCommand, AnalysisDataSet>, YahooFinanceAnalysisScrapeService>();
            services.AddTransient<IScrapeServiceStrategy<YahooFinanceCashFlowScraperCommand, CashFlowDataSet>, YahooFinanceCashFlowScrapeService>();
            services.AddTransient<IScrapeServiceStrategy<TripleABondYieldScraperCommand, TripleABondsDataSet>, YChartsTripleABondsScrapeService>();
            services.AddTransient<IScrapeServiceStrategy<BalanceSheetScraperCommand, BalanceSheetDataSet>, StockAnalysisBalanceSheetScrapeService>();
            services.AddTransient<IScrapeServiceStrategy<StatisticsScraperCommand, StatisticsDataSet>, StockAnalysisStatisticsScraperService>();
        }
    }
}
