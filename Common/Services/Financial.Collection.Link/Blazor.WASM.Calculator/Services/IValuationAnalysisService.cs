using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.IntrinsicValue.Calculation.Results;
using Financial.Collection.Domain.DTOs;
using Financial.Collection.Link.FinanceScraper.Encapsulation;
using Financial.Collection.Link.IntrinsicValue.Calculation.Encapsulator;

namespace Financial.Collection.Link.Blazor.WASM.Calculator.Services
{
    public interface IValuationAnalysisService
    {
        Task<MethodResultExceptionSum<TickerDto>> PerformAnalysis(ScraperParameterEncapsulator parameters);
    }
}
