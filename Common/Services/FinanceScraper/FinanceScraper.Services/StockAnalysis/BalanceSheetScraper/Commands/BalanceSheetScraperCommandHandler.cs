using MediatR;
using StockPortfolio.FinanceScraper.Common.Base;
using StockPortfolio.FinanceScraper.Common.DataSets;
using StockPortfolio.FinanceScraper.YahooFinance.AnalysisScraper.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPortfolio.FinanceScraper.StockAnalysis.BalanceSheetScraper.Commands
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
