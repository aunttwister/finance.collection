﻿using FinanceScraper.Common.Init.Commands;
using IntrinsicValue.Calculation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Finance.Collection.Domain.FinanceScraper.Results;
using Finance.Collection.Domain.Common.Propagation;
using Financial.Collection.Link.FinanceScraper.ServiceRegistar;

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

            InitScrapeCommand request = new InitScrapeCommand()
            {
                Ticker = ticker1,
                ExecuteDCFScrape = executeDcf,
                ExecuteGrahamScrape = executeGraham
            };

            InitScrapeCommand request2 = new InitScrapeCommand()
            {
                Ticker = ticker2,
                ExecuteDCFScrape = executeDcf,
                ExecuteGrahamScrape = executeGraham
            };
            InitScrapeCommand request3 = new InitScrapeCommand()
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
