using HtmlAgilityPack;
using FinanceScraper.Common.Base;
using FinanceScraper.Common.DataSets;
using FinanceScraper.Common.Exceptions;
using FinanceScraper.Common.Exceptions.ExceptionResolver;
using FinanceScraper.Common.Extensions;
using FinanceScraper.YahooFinance.AnalysisScraper.Commands;

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
            HtmlNode node = await request.NodeResolverAsync();

            Task<decimal> fiveYearGrowth = Task.Run(() => GetFiveYearGrowth(node, request.Ticker));

            await Task.WhenAll(fiveYearGrowth);

            return new AnalysisDataSet() { FiveYearGrowth = fiveYearGrowth.Result };
        }

        private decimal GetFiveYearGrowth(HtmlNode node, string ticker)
        {
            string commonExceptionSuffix = string.Format(" while attempting to acquire Growth Esimates for the next five years for the following ticker: {0}.", ticker);

            HtmlNode growthFiveNode = node.SelectSingleNode("//span[text()='Next 5 Years (per annum)']/../following-sibling::td[1]");

            _exceptionResolverService.HtmlNodeNullReferenceExceptionResolver(growthFiveNode, commonExceptionSuffix);
            _exceptionResolverService.HtmlNodeNotApplicableExceptionResolver(growthFiveNode, commonExceptionSuffix);

            char splitChar = '%';
            _exceptionResolverService.HtmlNodeKeyCharacterNotFoundExceptionResolver(growthFiveNode, splitChar, commonExceptionSuffix);

            string growthFiveStr = growthFiveNode.InnerHtml.Split(splitChar)[0];

            decimal growthFive = _exceptionResolverService.ConvertToDecimalExceptionResolver(growthFiveStr, commonExceptionSuffix);

            return growthFive;
        }
    }
}
