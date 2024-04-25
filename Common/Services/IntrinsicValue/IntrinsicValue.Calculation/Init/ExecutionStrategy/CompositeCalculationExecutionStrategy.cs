using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.FinanceScraper.Results;
using Finance.Collection.Domain.IntrinsicValue.Calculation.Results;
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

        public async Task<MethodResult<ICalculationResult>> ExecuteCalculationStrategy()
        {
            var tasks = _strategies.Select(strategy => strategy.ExecuteCalculationStrategy()).ToList();
            var results = await Task.WhenAll(tasks).ConfigureAwait(false);

            // Combine results, or return a new result that indicates the completion of all tasks.
            Finance.Collection.Domain.IntrinsicValue.Calculation.Results.CombinedCalculationResult combinedResult = new Finance.Collection.Domain.IntrinsicValue.Calculation.Results.CombinedCalculationResult();
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
