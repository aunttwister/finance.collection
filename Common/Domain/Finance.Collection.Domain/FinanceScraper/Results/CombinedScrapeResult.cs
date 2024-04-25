using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Collection.Domain.FinanceScraper.Results
{
    public class CombinedScrapeResult : IScrapeResult
    {
        public string Ticker { get; set; }
        public decimal CurrentPrice { get; set; }
        private readonly Dictionary<Type, IScrapeResult> _results;

        public CombinedScrapeResult()
        {
            _results = new Dictionary<Type, IScrapeResult>();
        }

        public void AddResult(IScrapeResult result)
        {
            _results[result.GetType()] = result;
        }

        public T GetResult<T>() where T : IScrapeResult
        {
            if (_results.TryGetValue(typeof(T), out IScrapeResult result))
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
