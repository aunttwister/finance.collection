using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.FinanceScraper.Results;
using Finance.Collection.Domain.IntrinsicValue.Calculation.Results;
using FinanceScraper.Common.Init.Commands;
using IntrinsicValue.Blazor.Services.FinanceServices.Interfaces;
using IntrinsicValue.Calculation.Init.Commands;
using MediatR;

namespace IntrinsicValue.Blazor.Services.FinanceServices.Services
{
    public class ValueCalculationService : IValueCalculationService
    {
        private readonly IMediator _mediator;
        public ValueCalculationService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<MethodResult<ICalculationResult>> CalculateFinancialDataAsync(InitCalculationCommand request)
        {
            Task<MethodResult<ICalculationResult>> result = _mediator.Send(request);

            return await result.ConfigureAwait(false);
        }
    }
}
