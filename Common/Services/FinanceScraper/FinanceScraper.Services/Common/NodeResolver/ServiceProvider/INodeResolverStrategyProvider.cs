using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScraper.Common.NodeResolver.ServiceProvider
{
    public interface INodeResolverStrategyProvider
    {
        INodeResolverStrategy GetCurrentStrategy();
        void SetCurrentStrategy(INodeResolverStrategy strategy);
    }
}
