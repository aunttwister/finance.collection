using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockPortfolio.FinanceScraper.Common.DataSets;
using StockPortfolio.FinanceScraper.Common.Base;

namespace StockPortfolio.FinanceScraper.MacroTrends.CashFlow.Commands
{
    public class MacroTrendsCashFlowScraperCommandHandler : IRequestHandler<MacroTrendsCashFlowScraperCommand, CashFlowDataSet>
    {
        private readonly IScrapeServiceStrategy<MacroTrendsCashFlowScraperCommand, CashFlowDataSet> _scrapeService;

        public MacroTrendsCashFlowScraperCommandHandler(IScrapeServiceStrategy<MacroTrendsCashFlowScraperCommand, CashFlowDataSet> scrapeService)
        {
            _scrapeService = scrapeService;
        }

        public async Task<CashFlowDataSet> Handle(MacroTrendsCashFlowScraperCommand request, CancellationToken cancellationToken)
        {
            return await _scrapeService.ExecuteScrape(request);
        }
    }
}
