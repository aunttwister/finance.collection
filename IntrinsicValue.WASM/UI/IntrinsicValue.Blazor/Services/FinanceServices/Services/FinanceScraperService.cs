using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.FinanceScraper.Results;
using FinanceScraper.Common.Init.Commands;
using IntrinsicValue.Blazor.Services.FinanceServices.Interfaces;
using MediatR;

namespace IntrinsicValue.Blazor.Services.FinanceServices.Services
{
    public class FinanceScraperService : IFinanceScraperService
    {
        private readonly IMediator _mediator;
        public FinanceScraperService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<MethodResult<IScrapeResult>> ScrapeFinancialDataAsync(InitScrapeCommand request)
        {
            Task<MethodResult<IScrapeResult>> result = _mediator.Send(request);

            return await result.ConfigureAwait(false);
        }
    }
}
