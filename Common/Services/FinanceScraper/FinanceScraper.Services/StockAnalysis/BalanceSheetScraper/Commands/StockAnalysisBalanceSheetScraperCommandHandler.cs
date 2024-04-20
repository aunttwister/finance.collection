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
    public class StockAnalysisBalanceSheetScraperCommandHandler : IRequestHandler<StockAnalysisBalanceSheetScraperCommand, BalanceSheetDataSet>
    {
        private readonly IScrapeServiceStrategy<StockAnalysisBalanceSheetScraperCommand, BalanceSheetDataSet> _scrapeService;

        public StockAnalysisBalanceSheetScraperCommandHandler(IScrapeServiceStrategy<StockAnalysisBalanceSheetScraperCommand, BalanceSheetDataSet> scrapeService)
        {
            _scrapeService = scrapeService;
        }

        public async Task<BalanceSheetDataSet> Handle(StockAnalysisBalanceSheetScraperCommand request, CancellationToken cancellationToken)
        {
            return await _scrapeService.ExecuteScrape(request);
        }
    }
}
