using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using IntrinsicValue.Calculation;
using Financial.Collection.Link.FinanceScraper.ServiceRegistar;
using System.Reflection;
using MudBlazor;
using IntrinsicValue.Blazor.Services.StateManagement;
using Financial.Collection.Link.Blazor.WASM.Calculator.ServiceRegistar;
using Financial.Collection.Link.IntrinsicValue.Calculation.ServiceRegistar;

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

            builder.Services.AddFinanceScraperServices();
            builder.Services.AddFinanceScraperHttpClient();

            builder.Services.AddValuationServices();

            builder.Services.AddIntrinsicCalculatorServices();

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
