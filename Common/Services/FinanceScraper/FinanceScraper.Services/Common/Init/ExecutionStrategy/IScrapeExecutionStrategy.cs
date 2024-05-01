using Finance.Collection.Domain.FinanceScraper.Results;
using FinanceScraper.Common.Init.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finance.Collection.Domain.Common.Propagation;
using FinanceScraper.Common.NodeResolver;

namespace FinanceScraper.Common.Init.ExecutionStrategy
{
    public interface IScrapeExecutionStrategy
    {
        public Task<MethodResult<IScrapeResult>> ExecuteScrapeStrategy();
    }
}
