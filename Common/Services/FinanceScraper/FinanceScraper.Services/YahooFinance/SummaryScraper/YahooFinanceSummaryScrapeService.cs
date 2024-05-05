using HtmlAgilityPack;
using FinanceScraper.Common.Base;
using FinanceScraper.Common.Exceptions;
using FinanceScraper.Common.Exceptions.ExceptionResolver;
using FinanceScraper.Common.Extensions;
using FinanceScraper.YahooFinance.SummaryScraper.Commands;
using System.Xml.Linq;
using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.FinanceScraper.DataSets;
using FinanceScraper.Common.NodeResolver.ServiceProvider;
using FinanceScraper.Common.NodeResolver;

namespace FinanceScraper.YahooFinance.SummaryScraper
{
    public class YahooFinanceSummaryScrapeService : IScrapeServiceStrategy<YahooFinanceSummaryScraperCommand, SummaryDataSet>
    {
        private readonly IExceptionResolverService _exceptionResolverService;
        private readonly INodeResolverStrategyProvider _nodeResolverStrategyProvider;
        public YahooFinanceSummaryScrapeService(IExceptionResolverService exceptionResolverService,
            INodeResolverStrategyProvider nodeResolverStrategyProvider)
        {
            _exceptionResolverService = exceptionResolverService;
            _nodeResolverStrategyProvider = nodeResolverStrategyProvider;
        }
        public async Task<SummaryDataSet> ExecuteScrape(YahooFinanceSummaryScraperCommand request)
        {
            INodeResolverStrategy nodeResolverStrategy = _nodeResolverStrategyProvider.GetCurrentStrategy();
            HtmlNode node = await nodeResolverStrategy.ResolveNodeAsync(request.FullUrl).ConfigureAwait(false);

            Task<MethodResult<decimal>> eps = Task.Run(() => GetEPS(node));

            await eps.ConfigureAwait(false);

            return new SummaryDataSet() { Eps = eps.Result };
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
