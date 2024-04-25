using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceScraper.Common.Base;
using Finance.Collection.Domain.FinanceScraper.DataSets;

namespace FinanceScraper.MacroTrends.CashFlow.Commands
{
    public class MacroTrendsCashOnHandScraperCommandHandler : IRequestHandler<MacroTrendsCashOnHandScraperCommand, CashFlowDataSet>
    {
        private readonly IScrapeServiceStrategy<MacroTrendsCashOnHandScraperCommand, CashFlowDataSet> _scrapeService;

        public MacroTrendsCashOnHandScraperCommandHandler(IScrapeServiceStrategy<MacroTrendsCashOnHandScraperCommand, CashFlowDataSet> scrapeService)
        {
            _scrapeService = scrapeService;
        }

        public async Task<CashFlowDataSet> Handle(MacroTrendsCashOnHandScraperCommand request, CancellationToken cancellationToken)
        {
            return await _scrapeService.ExecuteScrape(request);
        }
    }
}
