using HtmlAgilityPack;
using FinanceScraper.Common.Base;
using FinanceScraper.Common.DataSets;
using FinanceScraper.Common.Exceptions;
using FinanceScraper.Common.Exceptions.ExceptionResolver;
using FinanceScraper.Common.Extensions;
using FinanceScraper.YahooFinance.SummaryScraper.Commands;
using System.Xml.Linq;
using FinanceScraper.Common.Propagation;

namespace FinanceScraper.YahooFinance.SummaryScraper
{
    public class YahooFinanceSummaryScrapeService : IScrapeServiceStrategy<SummaryScraperCommand, SummaryDataSet>
    {
        private readonly IExceptionResolverService _exceptionResolverService;
        public YahooFinanceSummaryScrapeService(IExceptionResolverService exceptionResolverService)
        {
            _exceptionResolverService = exceptionResolverService;
        }
        public async Task<SummaryDataSet> ExecuteScrape(SummaryScraperCommand request)
        {
            HtmlNode node = await request.NodeResolverAsync().ConfigureAwait(false);

            Task<MethodResult<decimal>> currentPrice = Task.Run(() => GetCurrentPrice(node));

            Task<MethodResult<decimal>> eps = Task.Run(() => GetEPS(node));

            await Task.WhenAll(currentPrice, eps).ConfigureAwait(false);

            return new SummaryDataSet() { CurrentPrice = currentPrice.Result, Eps = eps.Result };
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
        [HandleMethodExecutionAspect]
        private async Task<MethodResult<decimal>> GetEPS(HtmlNode node)
        {
            try
            {
                Task<MethodResult<decimal>> taskEPS = Task.Run(() => ResolveEPS(node));

                await taskEPS.ConfigureAwait(false);

                return taskEPS.Result;
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

        private MethodResult<decimal> ResolveEPS(HtmlNode node)
        {
            node = node.SelectSingleNode("//li/span[text()=\"EPS (TTM)\"]/following-sibling::span[1]/fin-streamer");

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
