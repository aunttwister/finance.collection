using HtmlAgilityPack;
using StockPortfolio.FinanceScraper.Common.Base;
using StockPortfolio.FinanceScraper.Common.DataSets;
using StockPortfolio.FinanceScraper.Common.Exceptions;
using StockPortfolio.FinanceScraper.Common.Exceptions.ExceptionResolver;
using StockPortfolio.FinanceScraper.Common.Extensions;
using StockPortfolio.FinanceScraper.YahooFinance.AnalysisScraper.Commands;

namespace StockPortfolio.FinanceScraper.YahooFinance.AnalysisScraper
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
