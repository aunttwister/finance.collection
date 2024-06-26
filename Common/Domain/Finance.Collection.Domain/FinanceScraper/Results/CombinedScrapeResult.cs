﻿using Finance.Collection.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Collection.Domain.FinanceScraper.Results
{
    public class CombinedScrapeResult : IScrapeResult, ICombinedResult
    {
        public string Ticker { get; set; }
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
        public Dictionary<Type, IScrapeResult> GetAllResults() =>
            _results;
        public IEnumerable<Type> GetResultTypes(Func<Type, bool> condition = null)
        {
            if (condition == null)
            {
                condition = key => true;
            }

            return _results.Keys.Where(condition).AsEnumerable();
        }
    }
}
