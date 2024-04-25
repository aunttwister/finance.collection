using Finance.Collection.Domain.FinanceScraper.Results;
using FinanceScraper.Common.Init.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FinanceScraper.Common.Extensions;
using IntrinsicValue.Calculation;
using Finance.Collection.Domain.Common.Propagation;

namespace FinanceScrape.Executable.InitDebug
{
    public class Program
    {
        static async Task Main()
        {
            DateTime startTime = DateTime.Now;
            IServiceProvider serviceProvider = CreateHostBuilder().Build().Services;
            IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();

            string ticker1 = "NVDA";

            bool executeDcf = true;
            bool executeGraham = true;

            InitCommand request = new InitCommand()
            {
                Ticker = ticker1,
                ExecuteDCFScrape = executeDcf,
                ExecuteGrahamScrape = executeGraham
            };

            Task<MethodResult<IScrapeResult>> result = _mediator.Send(request);

            await Task.WhenAll(result).ConfigureAwait(false);

            var combinedResult = result.Result;

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
