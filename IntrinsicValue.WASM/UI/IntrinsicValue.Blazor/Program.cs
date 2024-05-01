using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using FinanceScraper.Common.Extensions;
using IntrinsicValue.Calculation;
using Microsoft.Extensions.DependencyInjection;
using IntrinsicValue.Blazor.Services.StateManagement;
using IntrinsicValue.Blazor.Services.FinanceServices.Interfaces;
using IntrinsicValue.Blazor.Services.FinanceServices.Services;
using FinanceScraper.Common.Extensions.ServiceRegistar;
using System.Reflection;
using MudBlazor;

namespace IntrinsicValue.Blazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            // Register MediatR
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            builder.Services.RegisterFinanceScraperServices();
            builder.Services.RegisterIntrinsicCalculatorServices();

            builder.Services.AddScoped<IFinanceScraperService, FinanceScraperService>();
            builder.Services.AddScoped<IValueCalculationService, ValueCalculationService>();
            builder.Services.AddScoped<IFinancialAnalysisService, FinancialAnalysisService>();

            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddScoped<TickerStateService>();

            builder.Services.AddMudServices(config =>
            {
                config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;

                config.SnackbarConfiguration.PreventDuplicates = false;
                config.SnackbarConfiguration.NewestOnTop = false;
                config.SnackbarConfiguration.ShowCloseIcon = true;
                config.SnackbarConfiguration.VisibleStateDuration = 10000;
                config.SnackbarConfiguration.HideTransitionDuration = 500;
                config.SnackbarConfiguration.ShowTransitionDuration = 500;
                config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
            });

            await builder.Build().RunAsync();
        }
    }
}
