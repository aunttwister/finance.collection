using Finance.Collection.Domain.FinanceScraper.DataSets;
using FinanceScraper.Common.Base;
using FinanceScraper.Common.Exceptions.ExceptionResolver;
using FinanceScraper.Common.NodeResolver.ServiceProvider;
using FinanceScraper.StockAnalysis.BalanceSheetScraper.Commands;
using FinanceScraper.StockAnalysis.BalanceSheetScraper;
using FinanceScraper.StockAnalysis.CashFlowScraper.Commands;
using FinanceScraper.StockAnalysis.CashFlowScraper;
using FinanceScraper.StockAnalysis.StatisticsScraper.Commands;
using FinanceScraper.StockAnalysis.StatisticsScraper;
using FinanceScraper.YahooFinance.AnalysisScraper.Commands;
using FinanceScraper.YahooFinance.AnalysisScraper;
using FinanceScraper.YahooFinance.CashFlowScraper.Commands;
using FinanceScraper.YahooFinance.CashFlowScraper;
using FinanceScraper.YahooFinance.CurrentPriceScraper.Commands;
using FinanceScraper.YahooFinance.CurrentPriceScraper;
using FinanceScraper.YahooFinance.SummaryScraper.Commands;
using FinanceScraper.YahooFinance.SummaryScraper;
using FinanceScraper.YCharts.TripleABondYieldScraper.Commands;
using FinanceScraper.YCharts.TripleABondYieldScraper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FinanceScraper.Common.Init.Commands;

namespace Financial.Collection.Link.FinanceScraper.ServiceRegistar
{
    public static class RegisterFinanceScraper
    {
        public static void AddFinanceScraperServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(InitScrapeCommand).Assembly));

            services.AddTransient<IExceptionResolverService, ExceptionResolverService>();

            services.AddScoped<INodeResolverStrategyProvider, NodeResolverStrategyProvider>();

            //services.AddTransient<IScrapeServiceStrategy<MacroTrendsCashFlowScraperCommand, CashFlowDataSet>, MacroTrendsCashFlowScrapeService>();
            services.AddTransient<IScrapeServiceStrategy<YahooFinanceSummaryScraperCommand, SummaryDataSet>, YahooFinanceSummaryScrapeService>();
            services.AddTransient<IScrapeServiceStrategy<YahooFinanceCurrentPriceScraperCommand, CurrentPriceDataSet>, YahooFinanceCurrentPriceScrapeService>();
            services.AddTransient<IScrapeServiceStrategy<AnalysisScraperCommand, AnalysisDataSet>, YahooFinanceAnalysisScrapeService>();
            services.AddTransient<IScrapeServiceStrategy<YahooFinanceCashFlowScraperCommand, CashFlowDataSet>, YahooFinanceCashFlowScrapeService>();
            services.AddTransient<IScrapeServiceStrategy<TripleABondYieldScraperCommand, TripleABondsDataSet>, YChartsTripleABondsScrapeService>();
            services.AddTransient<IScrapeServiceStrategy<StockAnalysisBalanceSheetScraperCommand, BalanceSheetDataSet>, StockAnalysisBalanceSheetScrapeService>();
            services.AddTransient<IScrapeServiceStrategy<StockAnalysisStatisticsScraperCommand, StatisticsDataSet>, StockAnalysisStatisticsScrapeService>();
            services.AddTransient<IScrapeServiceStrategy<StockAnalysisCashFlowScraperCommand, CashFlowDataSet>, StockAnalysisCashFlowScrapeService>();
        }
    }
}
