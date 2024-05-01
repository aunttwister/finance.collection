using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.FinanceScraper.Results;
using Finance.Collection.Domain.IntrinsicValue.Calculation.Results;
using IntrinsicValue.Calculation.Init.Commands;

namespace IntrinsicValue.Blazor.Services.FinanceServices.Interfaces
{
    public interface IValueCalculationService
    {
        public Task<MethodResult<ICalculationResult>> CalculateFinancialDataAsync(InitCalculationCommand request);
    }
}
