using Financial.Collection.Link.Blazor.WASM.Calculator.Services;
using Financial.Collection.Link.FinanceScraper.Services;
using Financial.Collection.Link.IntrinsicValue.Calculation.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Collection.Link.Blazor.WASM.Calculator.ServiceRegistar
{
    public static class RegisterIntrinsicCalculatorServices
    {
        public static void AddIntrinsicCalculatorServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(RegisterIntrinsicCalculatorServices).Assembly);

            services.AddScoped<IFinanceScraperService, FinanceScraperService>();
            services.AddScoped<IValueCalculationService, ValueCalculationService>();
            services.AddScoped<IValuationAnalysisService, ValuationAnalysisService>();
        }
    }
}
