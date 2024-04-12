using StockPortfolio.FinanceScraper.YahooFinance.SummaryScraper.Commands;
using StockPortfolio.FinanceScraper.YCharts.TripleABondYieldScraper.Commands;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockPortfolio.FinanceScraper.Common.Extensions;
using StockPortfolio.FinanceScraper.Common.Exceptions.ExceptionResolver;
using StockPortfolio.FinanceScraper.Common.DataSets;
using StockPortfolio.FinanceScraper.Common.Base;

namespace StockPortfolio.FinanceScraper.YCharts.TripleABondYieldScraper
{
    public class YChartsTripleABondsScrapeService : IScrapeServiceStrategy<TripleABondYieldScraperCommand, TripleABondsDataSet>
    {
        private readonly IExceptionResolverService _exceptionResolverService;
        public YChartsTripleABondsScrapeService(IExceptionResolverService exceptionResolverService)
        {
            _exceptionResolverService = exceptionResolverService;
        }
        public async Task<TripleABondsDataSet> ExecuteScrape(TripleABondYieldScraperCommand request)
        {
            HtmlNode node = await request.NodeResolverAsync();

            Task<decimal> currentTripleABond = Task.Run(() => GetCurrentBondTripleAYield(node));

            Task<decimal> historicalAverageTripleABond = Task.Run(() => GetHistoricalAverageBondTripleAYield(node));

            await Task.WhenAll(currentTripleABond, historicalAverageTripleABond);

            return new TripleABondsDataSet() { CurrentTripleABond = currentTripleABond.Result, HistoricalAverageTripleABond = historicalAverageTripleABond.Result };
        }

        private decimal GetCurrentBondTripleAYield(HtmlNode node)
        {
            string commonExceptionSuffix = string.Format(" while attempting to acquire Current AAA Bond Yield");

            HtmlNode currentBondYieldNode = node.SelectSingleNode("//td[text()='Last Value']/following-sibling::td[1]");

            _exceptionResolverService.HtmlNodeNullReferenceExceptionResolver(currentBondYieldNode, commonExceptionSuffix);

            char splitChar = '%';
            _exceptionResolverService.HtmlNodeKeyCharacterNotFoundExceptionResolver(currentBondYieldNode, splitChar, commonExceptionSuffix);
            string currentBondYieldStr = currentBondYieldNode.InnerHtml.Split(splitChar)[0];

            decimal currentBondYield = _exceptionResolverService.ConvertToDecimalExceptionResolver(currentBondYieldStr, commonExceptionSuffix);

            return currentBondYield / 100;
        }

        private decimal GetHistoricalAverageBondTripleAYield(HtmlNode node)
        {
            string commonExceptionSuffix = string.Format(" while attempting to acquire Historical Average AAA Bond Yield");

            HtmlNode averageBondYieldNode = node.SelectSingleNode("//td[text()='Long Term Average']/following-sibling::td[1]");

            _exceptionResolverService.HtmlNodeNullReferenceExceptionResolver(averageBondYieldNode, commonExceptionSuffix);

            char splitChar = '%';
            _exceptionResolverService.HtmlNodeKeyCharacterNotFoundExceptionResolver(averageBondYieldNode, splitChar, commonExceptionSuffix);
            string averageBondYieldStr = averageBondYieldNode.InnerHtml.Split(splitChar)[0];

            decimal averageBondYield = _exceptionResolverService.ConvertToDecimalExceptionResolver(averageBondYieldStr, commonExceptionSuffix);

            return averageBondYield / 100;
        }
    }
}
