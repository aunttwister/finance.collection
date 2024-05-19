using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using IntrinsicValue.Calculation;
using Financial.Collection.Link.FinanceScraper.ServiceRegistar;
using System.Reflection;
using MudBlazor;
using Financial.Collection.Link.Blazor.WASM.Calculator.ServiceRegistar;
using Financial.Collection.Link.IntrinsicValue.Calculation.ServiceRegistar;
using Intrinsicly.WASM;
using Intrinsicly.WASM.Services.Markdown;
using Intrinsicly.WASM.Services.LocalStorage;

namespace Intrinsicly.WASM
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5001/") });

            // Register MediatR
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            builder.Services.AddFinanceScraperServices();
            builder.Services.AddFinanceScraperHttpClient();

            builder.Services.AddValuationServices();

            builder.Services.AddIntrinsicCalculatorServices();

            builder.Services.AddAutoMapper(typeof(Program));

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

            builder.Services.AddMudMarkdownServices();
            builder.Services.AddScoped<MarkdownService>();
            builder.Services.AddScoped<LocalStorageService>();

            await builder.Build().RunAsync();
        }
    }
}
