using Finance.Collection.Domain.FinanceScraper.Results;
using FinanceScraper.Common.Init.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IntrinsicValue.Calculation;
using Finance.Collection.Domain.Common.Propagation;
using IntrinsicValue.Calculation.Init.Commands;
using Finance.Collection.Domain.IntrinsicValue.Calculation.Results;
using FinanceScraper.Common.Extensions.ServiceRegistar;

namespace FinanceScrape.Executable.InitDebug
{
    public class Program
    {
        static async Task Main()
        {
            DateTime startTime = DateTime.Now;
            IServiceProvider serviceProvider = CreateHostBuilder().Build().Services;
            IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();

            string ticker1 = "NTAP";

            bool executeDcf = true;
            bool executeGraham = true;

            InitScrapeCommand request = new InitScrapeCommand()
            {
                Ticker = ticker1,
                ExecuteDCFScrape = executeDcf,
                ExecuteGrahamScrape = executeGraham,
                UseHtmlContent = true
            };

            Task<MethodResult<IScrapeResult>> result = _mediator.Send(request);

            await result.ConfigureAwait(false);

            var combinedResult = result.Result;

            decimal safetyMargin = 0.65m;

            InitCalculationCommand requestCalc = new InitCalculationCommand()
            {
                Ticker = combinedResult.Data.Ticker,
                ScrapeResult = combinedResult.Data,
                SafetyMargin = safetyMargin
            };

            Task <MethodResult<ICalculationResult>> resultCalc = _mediator.Send(requestCalc);

            await resultCalc.ConfigureAwait(false);

            var calcResult = resultCalc.Result;

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
                           services.RegisterFinanceScraperServices();
                           services.RegisterIntrinsicCalculatorServices();
                       });
        }
    }
}
