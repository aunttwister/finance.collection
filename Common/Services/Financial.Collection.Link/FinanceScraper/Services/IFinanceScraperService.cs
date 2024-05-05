using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.FinanceScraper.Results;
using FinanceScraper.Common.Init.Commands;

namespace Financial.Collection.Link.FinanceScraper.Services
{
    public interface IFinanceScraperService
    {
        Task<MethodResult<IScrapeResult>> ScrapeFinancialDataAsync(InitScrapeCommand request);
    }
}
