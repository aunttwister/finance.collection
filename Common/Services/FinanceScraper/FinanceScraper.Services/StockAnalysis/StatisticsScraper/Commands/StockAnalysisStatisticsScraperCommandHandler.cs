using MediatR;
using FinanceScraper.Common.Base;
using FinanceScraper.Common.DataSets;
using FinanceScraper.StockAnalysis.BalanceSheetScraper.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScraper.StockAnalysis.StatisticsScraper.Commands
{
    public class StockAnalysisStatisticsScraperCommandHandler : IRequestHandler<StockAnalysisStatisticsScraperCommand, StatisticsDataSet>
    {
        private readonly IScrapeServiceStrategy<StockAnalysisStatisticsScraperCommand, StatisticsDataSet> _scrapeService;

        public StockAnalysisStatisticsScraperCommandHandler(IScrapeServiceStrategy<StockAnalysisStatisticsScraperCommand, StatisticsDataSet> scrapeService)
        {
            _scrapeService = scrapeService;
        }

        public async Task<StatisticsDataSet> Handle(StockAnalysisStatisticsScraperCommand request, CancellationToken cancellationToken)
        {
            return await _scrapeService.ExecuteScrape(request);
        }
    }
}
