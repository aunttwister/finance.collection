using HtmlAgilityPack;
using FinanceScraper.Common.Base;
using Finance.Collection.Domain.FinanceScraper.DataSets;
using FinanceScraper.Common.Exceptions;
using FinanceScraper.Common.Exceptions.ExceptionResolver;
using FinanceScraper.Common.Extensions;
using FinanceScraper.YahooFinance.AnalysisScraper.Commands;
using Finance.Collection.Domain.FinanceScraper.Propagation;

namespace FinanceScraper.YahooFinance.AnalysisScraper
{
    public class YahooFinanceAnalysisScrapeService : IScrapeServiceStrategy<AnalysisScraperCommand, AnalysisDataSet>
    {
        private readonly IExceptionResolverService _exceptionResolverService;
        public YahooFinanceAnalysisScrapeService(IExceptionResolverService exceptionResolverService) 
        {
            _exceptionResolverService = exceptionResolverService;
        }
        public async Task<AnalysisDataSet> ExecuteScrape(AnalysisScraperCommand request)
        {
            HtmlNode node = await request.NodeResolverAsync().ConfigureAwait(false);

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
