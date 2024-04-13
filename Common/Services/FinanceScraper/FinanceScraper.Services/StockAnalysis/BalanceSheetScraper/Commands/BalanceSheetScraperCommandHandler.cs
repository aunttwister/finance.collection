using MediatR;
using FinanceScraper.Common.Base;
using FinanceScraper.Common.DataSets;
using FinanceScraper.YahooFinance.AnalysisScraper.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScraper.StockAnalysis.BalanceSheetScraper.Commands
{
    public class BalanceSheetScraperCommandHandler : IRequestHandler<BalanceSheetScraperCommand, BalanceSheetDataSet>
    {
        private readonly IScrapeServiceStrategy<BalanceSheetScraperCommand, BalanceSheetDataSet> _scrapeService;

        public BalanceSheetScraperCommandHandler(IScrapeServiceStrategy<BalanceSheetScraperCommand, BalanceSheetDataSet> scrapeService)
        {
            _scrapeService = scrapeService;
        }

        public async Task<BalanceSheetDataSet> Handle(BalanceSheetScraperCommand request, CancellationToken cancellationToken)
        {
            return await _scrapeService.ExecuteScrape(request);
        }
    }
}
