using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Collection.Domain.FinanceScraper.Propagation
{
    public interface IOperationResult
    {
        Exception Exception { get; }
        bool IsSuccessful { get; }
    }
}
