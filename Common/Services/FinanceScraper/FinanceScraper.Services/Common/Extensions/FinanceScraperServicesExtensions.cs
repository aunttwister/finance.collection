using MediatR;
using Microsoft.Extensions.DependencyInjection;
using FinanceScraper.Common.Base;
using Finance.Collection.Domain.FinanceScraper.DataSets;
using FinanceScraper.Common.Exceptions.ExceptionResolver;
using FinanceScraper.MacroTrends.CashFlow;
using FinanceScraper.MacroTrends.CashFlow.Commands;
using FinanceScraper.StockAnalysis.BalanceSheetScraper;
using FinanceScraper.StockAnalysis.BalanceSheetScraper.Commands;
using FinanceScraper.StockAnalysis.StatisticsScraper;
using FinanceScraper.StockAnalysis.StatisticsScraper.Commands;
using FinanceScraper.YahooFinance.AnalysisScraper;
using FinanceScraper.YahooFinance.AnalysisScraper.Commands;
using FinanceScraper.YahooFinance.CashFlowScraper;
using FinanceScraper.YahooFinance.CashFlowScraper.Commands;
using FinanceScraper.YahooFinance.SummaryScraper;
using FinanceScraper.YahooFinance.SummaryScraper.Commands;
using FinanceScraper.YCharts.TripleABondYieldScraper;
using FinanceScraper.YCharts.TripleABondYieldScraper.Commands;
using System.Reflection;
using FinanceScraper.StockAnalysis.CashFlowScraper.Commands;
using FinanceScraper.StockAnalysis.CashFlowScraper;


namespace FinanceScraper.Common.Extensions
{
    public static class FinanceScraperServicesExtensions
    {
        public static void RegisterFinanceScraperServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            services.AddTransient<IExceptionResolverService, ExceptionResolverService>();

            //services.AddTransient<IScrapeServiceStrategy<MacroTrendsCashFlowScraperCommand, CashFlowDataSet>, MacroTrendsCashFlowScrapeService>();
            services.AddTransient<IScrapeServiceStrategy<SummaryScraperCommand, SummaryDataSet>, YahooFinanceSummaryScrapeService>();
            services.AddTransient<IScrapeServiceStrategy<AnalysisScraperCommand, AnalysisDataSet>, YahooFinanceAnalysisScrapeService>();
            services.AddTransient<IScrapeServiceStrategy<YahooFinanceCashFlowScraperCommand, CashFlowDataSet>, YahooFinanceCashFlowScrapeService>();
            services.AddTransient<IScrapeServiceStrategy<TripleABondYieldScraperCommand, TripleABondsDataSet>, YChartsTripleABondsScrapeService>();
            services.AddTransient<IScrapeServiceStrategy<StockAnalysisBalanceSheetScraperCommand, BalanceSheetDataSet>, StockAnalysisBalanceSheetScrapeService>();
            services.AddTransient<IScrapeServiceStrategy<StockAnalysisStatisticsScraperCommand, StatisticsDataSet>, StockAnalysisStatisticsScrapeService>();
            services.AddTransient<IScrapeServiceStrategy<StockAnalysisCashFlowScraperCommand, CashFlowDataSet>, StockAnalysisCashFlowScrapeService>();


        }
    }
}
