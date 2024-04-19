using MediatR;
using FinanceScraper.Common.Base;
using FinanceScraper.Common.DataSets;

namespace FinanceScraper.YCharts.TripleABondYieldScraper.Commands
{
    public class TripleABondYieldScraperCommandHandler : IRequestHandler<TripleABondYieldScraperCommand, TripleABondsDataSet>
    {
        private readonly IScrapeServiceStrategy<TripleABondYieldScraperCommand, TripleABondsDataSet> _scrapeService;

        public TripleABondYieldScraperCommandHandler(IScrapeServiceStrategy<TripleABondYieldScraperCommand, TripleABondsDataSet> scrapeService)
        {
            _scrapeService = scrapeService;
        }

        public async Task<TripleABondsDataSet> Handle(TripleABondYieldScraperCommand request, CancellationToken cancellationToken)
        {
            return await _scrapeService.ExecuteScrape(request);
        }
    }
}
