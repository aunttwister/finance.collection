using FinanceScraper.Common.DataSets.Results;
using FinanceScraper.Common.Init.Commands;
using FinanceScraper.Common.Propagation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScraper.Common.Init.ExecutionStrategy
{
    public interface IExecutionScrapeStrategy
    {
        public Task<MethodResult<IScrapeResult>> ExecuteScrapeStrategy();
    }
}
