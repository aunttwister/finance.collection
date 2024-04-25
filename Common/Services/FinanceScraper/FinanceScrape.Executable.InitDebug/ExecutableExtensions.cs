using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScrape.Executable.InitDebug
{
    public static class ExecutableExtensions
    {
        public static void RegisterMediatR(this IServiceCollection services)
        {
            /*services.AddMediatR(options =>
            {
                options.AsTransient();
            },
            Assembly.GetExecutingAssembly());*/

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            /*services.AddAutoMapper(typeof(FinanceScraperServiceExtensions).Assembly);*/
        }
    }
}
