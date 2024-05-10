using Finance.Collection.Domain.FinanceScraper.Results;
using FinanceScraper.Common.Init.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Finance.Collection.Domain.Common.Propagation;
using IntrinsicValue.Calculation.Init.Commands;
using Finance.Collection.Domain.IntrinsicValue.Calculation.Results;
using Financial.Collection.Link.FinanceScraper.ServiceRegistar;
using Financial.Collection.Link.IntrinsicValue.Calculation.ServiceRegistar;
using Financial.Collection.Link.Blazor.WASM.Calculator.ServiceRegistar;
using Financial.Collection.Link.Blazor.WASM.Calculator.Services;
using Financial.Collection.Link.FinanceScraper.Encapsulation;
using Financial.Collection.Domain.DTOs;
using Finance.Collection.Domain.DTOs.Results;
using Financial.Collection.Link.IntrinsicValue.Calculation.Encapsulator;

namespace FinanceScrape.Executable.InitDebug
{
    public class Program
    {
        static async Task Main()
        {
            DateTime startTime = DateTime.Now;
            IServiceProvider serviceProvider = CreateHostBuilder().Build().Services;
            IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();
            IValuationAnalysisService _valuationAnalysisService = serviceProvider.GetRequiredService<IValuationAnalysisService>();

            string ticker1 = "NTDOY";

            bool executeGraham = true;
            bool executeDcf = true;

            decimal safetyMargin = 65m;

            ScraperParameterEncapsulator request = new ScraperParameterEncapsulator()
            {
                Ticker = ticker1,
                ExecuteGrahamScrape = executeGraham,
                ExecuteDCFScrape = executeDcf,
                UseHtmlContent = false
            };

            ScrapeResultDto scrapeResultDto = await _valuationAnalysisService.PerformScrape(request);

            CalculationParameterEncapsulator calculationParameterEncapsulator = new CalculationParameterEncapsulator()
            {
                TickerDto = scrapeResultDto.TickerDto,
                AAABondDto = scrapeResultDto.AAABondDto,
                ExecuteGrahamCalculation = executeGraham,
                ExecuteDCFCalculation = executeDcf,
                SafetyMargin = safetyMargin
            };

            CalculationResultDto calculationResultDto = await _valuationAnalysisService.PerformValuation(calculationParameterEncapsulator);

            Console.Write($"Run time: {DateTime.Now - startTime}");
            Console.WriteLine();

            Console.ReadLine();
        }

        public static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                       .ConfigureServices((hostContext, services) =>
                       {
                           services.RegisterMediatR();
                           services.AddFinanceScraperServices();
                           services.AddFinanceScraperHttpClient();
                           services.AddValuationServices();

                           services.AddIntrinsicCalculatorServices();
                       });
        }
    }
}
