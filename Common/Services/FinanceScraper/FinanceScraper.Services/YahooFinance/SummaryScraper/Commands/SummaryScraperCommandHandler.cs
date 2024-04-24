using MediatR;
using FinanceScraper.Common.Base;
using Finance.Collection.Domain.FinanceScraper.DataSets;

namespace FinanceScraper.YahooFinance.SummaryScraper.Commands
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
