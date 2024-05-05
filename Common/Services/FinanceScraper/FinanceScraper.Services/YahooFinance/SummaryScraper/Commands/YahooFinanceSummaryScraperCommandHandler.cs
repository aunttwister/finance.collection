using MediatR;
using FinanceScraper.Common.Base;
using Finance.Collection.Domain.FinanceScraper.DataSets;

namespace FinanceScraper.YahooFinance.SummaryScraper.Commands
{
    public class YahooFinanceSummaryScraperCommandHandler : IRequestHandler<YahooFinanceSummaryScraperCommand, SummaryDataSet>
    {
        private readonly IScrapeServiceStrategy<YahooFinanceSummaryScraperCommand, SummaryDataSet> _scrapeService;

        public YahooFinanceSummaryScraperCommandHandler(IScrapeServiceStrategy<YahooFinanceSummaryScraperCommand, SummaryDataSet> scrapeService)
        {
            _scrapeService = scrapeService;
        }

        public async Task<SummaryDataSet> Handle(YahooFinanceSummaryScraperCommand request, CancellationToken cancellationToken)
        {
            return await _scrapeService.ExecuteScrape(request);
        }
    }
}
