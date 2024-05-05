using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.FinanceScraper.DataSets;
using FinanceScraper.Common.Base;
using FinanceScraper.YahooFinance.SummaryScraper.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScraper.YahooFinance.CurrentPriceScraper.Commands
{
    public class YahooFinanceCurrentPriceScraperCommandHandler : IRequestHandler<YahooFinanceCurrentPriceScraperCommand, CurrentPriceDataSet>
    {
        private readonly IScrapeServiceStrategy<YahooFinanceCurrentPriceScraperCommand, CurrentPriceDataSet> _scrapeService;

        public YahooFinanceCurrentPriceScraperCommandHandler(IScrapeServiceStrategy<YahooFinanceCurrentPriceScraperCommand, CurrentPriceDataSet> scrapeService)
        {
            _scrapeService = scrapeService;
        }

        public async Task<CurrentPriceDataSet> Handle(YahooFinanceCurrentPriceScraperCommand request, CancellationToken cancellationToken)
        {
            return await _scrapeService.ExecuteScrape(request);
        }
    }
}
