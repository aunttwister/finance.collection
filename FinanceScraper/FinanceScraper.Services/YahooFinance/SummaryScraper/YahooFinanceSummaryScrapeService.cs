using HtmlAgilityPack;
using StockPortfolio.FinanceScraper.Common.Base;
using StockPortfolio.FinanceScraper.Common.DataSets;
using StockPortfolio.FinanceScraper.Common.Exceptions;
using StockPortfolio.FinanceScraper.Common.Exceptions.ExceptionResolver;
using StockPortfolio.FinanceScraper.Common.Extensions;
using StockPortfolio.FinanceScraper.YahooFinance.SummaryScraper.Commands;

namespace StockPortfolio.FinanceScraper.YahooFinance.SummaryScraper
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
            HtmlNode node = await request.NodeResolverAsync();

            Task<decimal> currentPrice = Task.Run(() => GetCurrentPrice(node, request.Ticker));

            Task<decimal> eps = Task.Run(() => GetEPS(node, request.Ticker));

            await Task.WhenAll(currentPrice, eps);

            return new SummaryDataSet() { CurrentPrice = currentPrice.Result, Eps = eps.Result };
        }

        private decimal GetCurrentPrice(HtmlNode node, string ticker)
        {
            string commonExceptionSuffix = string.Format(" while attempting to acquire Current Price for the following ticker: {0}.", ticker);

            HtmlNode currentPriceNode = node.SelectSingleNode("//fin-streamer[@data-test='qsp-price']");
            
            _exceptionResolverService.HtmlNodeNullReferenceExceptionResolver(currentPriceNode, commonExceptionSuffix);
            decimal currentPrice = _exceptionResolverService.ConvertToDecimalExceptionResolver(currentPriceNode.InnerHtml, commonExceptionSuffix);

            return currentPrice;
        }

        private decimal GetEPS(HtmlNode node, string ticker)
        {
            string commonExceptionSuffix = string.Format(" while attempting to acquire EPS for the following ticker: {0}.", ticker);

            HtmlNode epsNode = node.SelectSingleNode("//td[@data-test='EPS_RATIO-value']");

            _exceptionResolverService.HtmlNodeNullReferenceExceptionResolver(epsNode, commonExceptionSuffix);
            _exceptionResolverService.HtmlNodeNotApplicableExceptionResolver(epsNode, commonExceptionSuffix);
            decimal eps = _exceptionResolverService.ConvertToDecimalExceptionResolver(epsNode.InnerHtml, commonExceptionSuffix);

            return eps;
        }
    }
}
