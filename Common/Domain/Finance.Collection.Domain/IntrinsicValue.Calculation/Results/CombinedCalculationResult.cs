using Finance.Collection.Domain.FinanceScraper.Results;
using Finance.Collection.Domain.IntrinsicValue.Calculation.DataSets.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Collection.Domain.IntrinsicValue.Calculation.Results
{
    public class CombinedCalculationResult : BaseIntrinsicModelDataSet, ICalculationResult
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
