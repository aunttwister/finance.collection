using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.FinanceScraper.Results;
using Finance.Collection.Domain.IntrinsicValue.Calculation.Results;
using Financial.Collection.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntrinsicValue.Calculation.Init.ExecutionStrategy
{
    public interface ICalculationExecutionStrategy
    {
        public Task<MethodResult<ICalculationResult>> ExecuteCalculationStrategy(TickerDto ticker, AAABondDto aaaBond, decimal safetyMargin);
    }
}
