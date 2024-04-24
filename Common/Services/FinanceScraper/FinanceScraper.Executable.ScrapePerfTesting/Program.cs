
using Finance.Collection.Domain.FinanceScraper;
using FinanceScraper.Common.Init.Commands;
using Finance.Collection.Domain.FinanceScraper.Propagation;
using IntrinsicValue.Calculation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FinanceScraper.Common.Extensions;
using Finance.Collection.Domain.FinanceScraper.Results;

namespace FinanceScraper.Executable.ScrapePerfTesting
{
    public class Program
    {
        static async Task Main()
        {
            DateTime startTime = DateTime.Now;
            IServiceProvider serviceProvider = CreateHostBuilder().Build().Services;
            IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();

            string ticker1 = "NVDA";
            string ticker2 = "META";
            string ticker3 = "MSFT";

            bool executeDcf = true;
            bool executeGraham = true;

            InitCommand request = new InitCommand()
            {
                Ticker = ticker1,
                ExecuteDCFScrape = executeDcf,
                ExecuteGrahamScrape = executeGraham
            };

            InitCommand request2 = new InitCommand()
            {
                Ticker = ticker2,
                ExecuteDCFScrape = executeDcf,
                ExecuteGrahamScrape = executeGraham
            };
            InitCommand request3 = new InitCommand()
            {
                Ticker = ticker3,
                ExecuteDCFScrape = executeDcf,
                ExecuteGrahamScrape = executeGraham
            };

            Task<MethodResult<IScrapeResult>> result = _mediator.Send(request);
            //Task<MethodResult<IScrapeResult>> result2 = _mediator.Send(request2);
            //Task<MethodResult<IScrapeResult>> result3 = _mediator.Send(request3);

            await Task.WhenAll(result
                //, result2
                //, result3
                ).ConfigureAwait(false);

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
