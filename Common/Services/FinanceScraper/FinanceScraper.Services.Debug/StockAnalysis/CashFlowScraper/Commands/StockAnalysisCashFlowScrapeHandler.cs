using FinanceScraper.Common.Base;
using FinanceScraper.Common.DataSets;
using FinanceScraper.StockAnalysis.BalanceSheetScraper.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScraper.StockAnalysis.CashFlowScraper.Commands
{
    public class StockAnalysisCashFlowScraperCommandHandler : IRequestHandler<StockAnalysisCashFlowScraperCommand, CashFlowDataSet>
    {
        private readonly IScrapeServiceStrategy<StockAnalysisCashFlowScraperCommand, CashFlowDataSet> _scrapeService;

        public StockAnalysisCashFlowScraperCommandHandler(IScrapeServiceStrategy<StockAnalysisCashFlowScraperCommand, CashFlowDataSet> scrapeService)
        {
            _scrapeService = scrapeService;
        }

        public async Task<CashFlowDataSet> Handle(StockAnalysisCashFlowScraperCommand request, CancellationToken cancellationToken)
        {
            return await _scrapeService.ExecuteScrape(request);
        }
    }
}
