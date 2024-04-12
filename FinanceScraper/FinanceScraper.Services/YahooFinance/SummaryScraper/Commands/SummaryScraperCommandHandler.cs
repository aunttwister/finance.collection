using MediatR;
using StockPortfolio.FinanceScraper.Common.Base;
using StockPortfolio.FinanceScraper.Common.DataSets;

namespace StockPortfolio.FinanceScraper.YahooFinance.SummaryScraper.Commands
{
    public class SummaryScraperCommandHandler : IRequestHandler<SummaryScraperCommand, SummaryDataSet>
    {
        private readonly IScrapeServiceStrategy<SummaryScraperCommand, SummaryDataSet> _scrapeService;

        public SummaryScraperCommandHandler(IScrapeServiceStrategy<SummaryScraperCommand, SummaryDataSet> scrapeService)
        {
            _scrapeService = scrapeService;
        }

        public async Task<SummaryDataSet> Handle(SummaryScraperCommand request, CancellationToken cancellationToken)
        {
            return await _scrapeService.ExecuteScrape(request);
        }
    }
}
