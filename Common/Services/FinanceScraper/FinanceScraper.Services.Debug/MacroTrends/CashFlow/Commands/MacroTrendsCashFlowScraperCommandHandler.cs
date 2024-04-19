using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceScraper.Common.DataSets;
using FinanceScraper.Common.Base;

namespace FinanceScraper.MacroTrends.CashFlow.Commands
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
