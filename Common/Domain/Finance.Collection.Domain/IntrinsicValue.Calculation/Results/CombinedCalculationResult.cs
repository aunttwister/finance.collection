using Finance.Collection.Domain.Common.Results;
using Finance.Collection.Domain.FinanceScraper.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Collection.Domain.IntrinsicValue.Calculation.Results
{
    public class CombinedCalculationResult : ICalculationResult, ICombinedResult
    {
        private readonly Dictionary<Type, ICalculationResult> _results;

        public CombinedCalculationResult()
        {
            _results = new Dictionary<Type, ICalculationResult>();
        }

        public void AddResult(ICalculationResult result)
        {
            _results[result.GetType()] = result;
        }

        public T GetResult<T>() where T : ICalculationResult
        {
            if (_results.TryGetValue(typeof(T), out ICalculationResult result))
            {
                return (T)result;
            }
            return default(T);
        }
        public IEnumerable<Type> GetResultTypes()
        {
            return _results.Keys.AsEnumerable();
        }
    }
}
