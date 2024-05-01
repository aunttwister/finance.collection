using HtmlAgilityPack;
using FinanceScraper.Common.Base;
using FinanceScraper.Common.Exceptions;
using FinanceScraper.Common.Exceptions.ExceptionResolver;
using FinanceScraper.Common.Extensions;
using FinanceScraper.YahooFinance.AnalysisScraper.Commands;
using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.FinanceScraper.DataSets;
using FinanceScraper.Common.NodeResolver.ServiceProvider;
using FinanceScraper.Common.NodeResolver;

namespace FinanceScraper.YahooFinance.AnalysisScraper
{
    public class YahooFinanceAnalysisScrapeService : IScrapeServiceStrategy<AnalysisScraperCommand, AnalysisDataSet>
    {
        private readonly IExceptionResolverService _exceptionResolverService;
        private readonly INodeResolverStrategyProvider _nodeResolverStrategyProvider;
        public YahooFinanceAnalysisScrapeService(IExceptionResolverService exceptionResolverService,
            INodeResolverStrategyProvider nodeResolverStrategyProvider)
        {
            _exceptionResolverService = exceptionResolverService;
            _nodeResolverStrategyProvider = nodeResolverStrategyProvider;
        }
        public async Task<AnalysisDataSet> ExecuteScrape(AnalysisScraperCommand request)
        {
            INodeResolverStrategy nodeResolverStrategy = _nodeResolverStrategyProvider.GetCurrentStrategy();
            HtmlNode node = await nodeResolverStrategy.ResolveNodeAsync(request.FullUrl).ConfigureAwait(false);

            Task<MethodResult<decimal>> fiveYearGrowth = Task.Run(() => GetFiveYearGrowth(node));

            await fiveYearGrowth.ConfigureAwait(false);

            return new AnalysisDataSet() { FiveYearGrowth = fiveYearGrowth.Result };
        }

        [HandleMethodExecutionAspect]
        private Task<MethodResult<decimal>> GetFiveYearGrowth(HtmlNode node)
        {
            try
            {
                return Task.Run(() => ResolveFiveYearGrowth(node));
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public MethodResult<decimal> ResolveFiveYearGrowth(HtmlNode node)
        {
            node = node.SelectSingleNode("//td[text()='Next 5 Years (per annum)']/following-sibling::td[1]");

            char splitChar = '%';

            Func<MethodResult<decimal>>[] operations = new Func<MethodResult<decimal>>[]
            {
                () => _exceptionResolverService.HtmlNodeNullReferenceExceptionResolver<decimal>(node),
                () => _exceptionResolverService.HtmlNodeNotApplicableExceptionResolver<decimal>(node),
                () => _exceptionResolverService.HtmlNodeKeyCharacterNotFoundExceptionResolver<decimal>(node, splitChar),
                () => _exceptionResolverService.ConvertToDecimalExceptionResolver(node.InnerHtml.Split(splitChar)[0])
            };

            return node.ExecuteUntilFirstException(operations);
        }
    }
}
