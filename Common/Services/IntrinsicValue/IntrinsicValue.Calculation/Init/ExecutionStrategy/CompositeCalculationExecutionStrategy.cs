using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.FinanceScraper.Results;
using Finance.Collection.Domain.IntrinsicValue.Calculation.Results;
using Financial.Collection.Domain.DTOs;
using IntrinsicValue.Calculation.Init.Commands;
using IntrinsicValue.Calculation.Init.ExecutionStrategy.Factory;
using MediatR;

namespace IntrinsicValue.Calculation.Init.ExecutionStrategy
{
    public class CompositeCalculationExecutionStrategy : ICalculationExecutionStrategy
    {
        private readonly List<ICalculationExecutionStrategy> _strategies;

        public CompositeCalculationExecutionStrategy(IEnumerable<ICalculationExecutionStrategy> strategies)
        {
            _strategies = strategies.ToList();
        }
        public async Task<MethodResult<ICalculationResult>> ExecuteCalculationStrategy(TickerDto tickerDto, AAABondDto aaaBond, decimal safetyMargin)
        {
            List<Task<MethodResult<ICalculationResult>>> tasks = new List<Task<MethodResult<ICalculationResult>>>();
            foreach (ICalculationExecutionStrategy strategy in _strategies)
            {
                tasks.Add(strategy.ExecuteCalculationStrategy(tickerDto, aaaBond, safetyMargin));
            }

            var results = await Task.WhenAll(tasks).ConfigureAwait(false);

            // Combine results, or return a new result that indicates the completion of all tasks.
            CombinedCalculationResult combinedResult = new CombinedCalculationResult();
            foreach (var result in results)
            {
                combinedResult.AddResult(result.Data);
            }

            MethodResult<ICalculationResult> finalResult = new MethodResult<ICalculationResult>();
            finalResult.AssignData(combinedResult);

            return finalResult;
        }
    }
}
