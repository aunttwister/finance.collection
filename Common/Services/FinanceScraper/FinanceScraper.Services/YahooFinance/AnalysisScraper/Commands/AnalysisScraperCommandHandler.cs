using FinanceScraper.YahooFinance.AnalysisScraper.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceScraper.Common.Base;
using Finance.Collection.Domain.FinanceScraper.DataSets;

namespace FinanceScraper.YahooFinance.AnalysisScraper.Commands
{
    public class AnalysisScraperCommandHandler : IRequestHandler<AnalysisScraperCommand, AnalysisDataSet>
    {
        private readonly IScrapeServiceStrategy<AnalysisScraperCommand, AnalysisDataSet> _scrapeService;

        public AnalysisScraperCommandHandler(IScrapeServiceStrategy<AnalysisScraperCommand, AnalysisDataSet> scrapeService)
        {
            _scrapeService = scrapeService;
        }

        public async Task<AnalysisDataSet> Handle(AnalysisScraperCommand request, CancellationToken cancellationToken)
        {
            return await _scrapeService.ExecuteScrape(request);
        }
    }
}
