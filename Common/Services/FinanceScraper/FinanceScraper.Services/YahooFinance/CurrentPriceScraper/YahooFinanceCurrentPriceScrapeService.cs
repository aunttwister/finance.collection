using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.FinanceScraper.DataSets;
using FinanceScraper.Common.Base;
using FinanceScraper.Common.Exceptions.ExceptionResolver;
using FinanceScraper.Common.NodeResolver.ServiceProvider;
using FinanceScraper.Common.NodeResolver;
using FinanceScraper.YahooFinance.SummaryScraper.Commands;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceScraper.Common.Extensions;
using FinanceScraper.YahooFinance.CurrentPriceScraper.Commands;

namespace FinanceScraper.YahooFinance.CurrentPriceScraper
{
    public class YahooFinanceCurrentPriceScrapeService : IScrapeServiceStrategy<YahooFinanceCurrentPriceScraperCommand, CurrentPriceDataSet>
    {
        private readonly IExceptionResolverService _exceptionResolverService;
        private readonly INodeResolverStrategyProvider _nodeResolverStrategyProvider;
        public YahooFinanceCurrentPriceScrapeService(IExceptionResolverService exceptionResolverService,
            INodeResolverStrategyProvider nodeResolverStrategyProvider)
        {
            _exceptionResolverService = exceptionResolverService;
            _nodeResolverStrategyProvider = nodeResolverStrategyProvider;
        }
        public async Task<CurrentPriceDataSet> ExecuteScrape(YahooFinanceCurrentPriceScraperCommand request)
        {
            INodeResolverStrategy nodeResolverStrategy = _nodeResolverStrategyProvider.GetCurrentStrategy();
            HtmlNode node = await nodeResolverStrategy.ResolveNodeAsync(request.FullUrl).ConfigureAwait(false);

            Task<MethodResult<decimal>> currentPrice = Task.Run(() => GetCurrentPrice(node));

            await currentPrice.ConfigureAwait(false);

                return new CurrentPriceDataSet() { CurrentPrice = currentPrice.Result };
        }
        [HandleMethodExecutionAspect]
        private async Task<MethodResult<decimal>> GetCurrentPrice(HtmlNode node)
        {
            try
            {
                Task<MethodResult<decimal>> taskCurrentPrice = Task.Run(() => ResolveCurrentPrice(node));

                await taskCurrentPrice.ConfigureAwait(false);

                return taskCurrentPrice.Result;
            }
            catch (Exception)
            {

                throw;
            }

        }
        private MethodResult<decimal> ResolveCurrentPrice(HtmlNode node)
        {
            node = node.SelectSingleNode("//fin-streamer[@data-testid='qsp-price']/span");

            Func<MethodResult<decimal>>[] operations = new Func<MethodResult<decimal>>[]
            {
                () => _exceptionResolverService.HtmlNodeNullReferenceExceptionResolver<decimal>(node),
                () => _exceptionResolverService.HtmlNodeNotApplicableExceptionResolver<decimal>(node),
                () => _exceptionResolverService.ConvertToDecimalExceptionResolver(node.InnerHtml)
            };

            return node.ExecuteUntilFirstException(operations);
        }

    }
}
