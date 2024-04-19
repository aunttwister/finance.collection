using MediatR;
using FinanceScraper.Common.Base;
using FinanceScraper.Common.DataSets;

namespace FinanceScraper.YahooFinance.CashFlowScraper.Commands
{
    public class YahooFinanceCashFlowScraperCommandHandler : IRequestHandler<YahooFinanceCashFlowScraperCommand, CashFlowDataSet>
    {
        private readonly IScrapeServiceStrategy<YahooFinanceCashFlowScraperCommand, CashFlowDataSet> _scrapeService;

        public YahooFinanceCashFlowScraperCommandHandler(IScrapeServiceStrategy<YahooFinanceCashFlowScraperCommand, CashFlowDataSet> scrapeService)
        {
            _scrapeService = scrapeService;
        }

        public async Task<CashFlowDataSet> Handle(YahooFinanceCashFlowScraperCommand request, CancellationToken cancellationToken)
        {
            return await _scrapeService.ExecuteScrape(request);
        }
    }
}
