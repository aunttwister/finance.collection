using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScraper.Common.NodeResolver.ServiceProvider
{
    public class NodeResolverStrategyProvider : INodeResolverStrategyProvider
    {
        private AsyncLocal<INodeResolverStrategy> _currentStrategy = new AsyncLocal<INodeResolverStrategy>();

        public INodeResolverStrategy GetCurrentStrategy()
        {
            return _currentStrategy.Value;
        }

        public void SetCurrentStrategy(INodeResolverStrategy strategy)
        {
            _currentStrategy.Value = strategy;
        }
    }
}
