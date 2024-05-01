using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.FinanceScraper.Results;
using FinanceScraper.Common.Init.Commands;

namespace IntrinsicValue.Blazor.Services.FinanceServices.Interfaces
{
    public interface IFinanceScraperService
    {
        Task<MethodResult<IScrapeResult>> ScrapeFinancialDataAsync(InitScrapeCommand request);
    }
}
