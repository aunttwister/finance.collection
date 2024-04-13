using MediatR;
using StockPortfolio.FinanceScraper.Common.Base;
using StockPortfolio.FinanceScraper.Common.DataSets;
using StockPortfolio.FinanceScraper.StockAnalysis.BalanceSheetScraper.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPortfolio.FinanceScraper.StockAnalysis.StatisticsScraper.Commands
{
    public class StatisticsScraperCommandHandler : IRequestHandler<StatisticsScraperCommand, StatisticsDataSet>
    {
        private readonly IScrapeServiceStrategy<StatisticsScraperCommand, StatisticsDataSet> _scrapeService;

        public StatisticsScraperCommandHandler(IScrapeServiceStrategy<StatisticsScraperCommand, StatisticsDataSet> scrapeService)
        {
            _scrapeService = scrapeService;
        }

        public async Task<StatisticsDataSet> Handle(StatisticsScraperCommand request, CancellationToken cancellationToken)
        {
            return await _scrapeService.ExecuteScrape(request);
        }
    }
}
