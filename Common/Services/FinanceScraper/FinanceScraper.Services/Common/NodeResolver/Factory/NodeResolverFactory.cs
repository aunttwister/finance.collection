using FinanceScraper.Common.Base;
using FinanceScraper.Common.Init.Commands;
using FinanceScraper.Common.NodeResolver.HttpClientFactory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScraper.Common.NodeResolver.Factory
{
    public class NodeResolverFactory
    {
        private static readonly IDictionary<string, INodeResolverStrategy> _strategies = new Dictionary<string, INodeResolverStrategy>();
        static NodeResolverFactory()
        {
            _strategies = new Dictionary<string, INodeResolverStrategy>
        {
            { "Web", new WebNodeResolverStrategy() },
            { "Content", new ContentNodeResolverStrategy(new HtmlContentClient()) }
        };
        }
        public static void Initialize(IServiceProvider serviceProvider)
        {
            // Register all strategies with their keys
            var webNodeResolver = serviceProvider.GetRequiredService<WebNodeResolverStrategy>();
            var contentNodeResolver = serviceProvider.GetRequiredService<ContentNodeResolverStrategy>();
            _strategies.Add("Web", webNodeResolver);
            _strategies.Add("Content", contentNodeResolver);
        }

        public INodeResolverStrategy GetResolverStrategy(bool useHtmlContent)
        {
            if (useHtmlContent)
                return _strategies["Content"];
            else
                return _strategies["Web"];
        }
    }
}
