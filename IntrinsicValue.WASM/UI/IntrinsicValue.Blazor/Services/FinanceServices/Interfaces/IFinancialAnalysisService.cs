using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.IntrinsicValue.Calculation.Results;
using IntrinsicValue.Blazor.Services.FinanceServices.Encapsulation;

namespace IntrinsicValue.Blazor.Services.FinanceServices.Interfaces
{
    public interface IFinancialAnalysisService
    {
        Task<MethodResult<ICalculationResult>> PerformAnalysis(CalculatorParameterEncapsulator parameters);
    }
}
