using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceScraper.Common.NodeResolver.HttpClientFactory;
using FinanceScraper.Common.NodeResolver.Factory;
using FinanceScraper.Common.NodeResolver;

namespace FinanceScraper.Common.Extensions.ServiceRegistar
{
    public static class FinanceScraperHttpClientExtensions
    {
        public static void RegisterHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient<HtmlContentClient>();
            services.AddTransient<WebNodeResolverStrategy>();
            services.AddTransient<ContentNodeResolverStrategy>();

            // Optionally, immediately initialize the factory after service registration
            using (var serviceProvider = services.BuildServiceProvider())
            {
                NodeResolverFactory.Initialize(serviceProvider);
            }
        }
    }
}
