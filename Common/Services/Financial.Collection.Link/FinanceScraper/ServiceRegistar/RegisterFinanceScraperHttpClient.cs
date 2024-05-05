using FinanceScraper.Common.NodeResolver.Factory;
using FinanceScraper.Common.NodeResolver.HttpClientFactory;
using FinanceScraper.Common.NodeResolver;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Collection.Link.FinanceScraper.ServiceRegistar
{
    public static class RegisterFinanceScraperHttpClient
    {
        public static IServiceCollection AddFinanceScraperHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient<HtmlContentClient>();
            services.AddTransient<WebNodeResolverStrategy>();
            services.AddTransient<ContentNodeResolverStrategy>();

            // Optionally, immediately initialize the factory after service registration
            using (var serviceProvider = services.BuildServiceProvider())
            {
                NodeResolverFactory.Initialize(serviceProvider);
            }

            return services;
        }
    }
}