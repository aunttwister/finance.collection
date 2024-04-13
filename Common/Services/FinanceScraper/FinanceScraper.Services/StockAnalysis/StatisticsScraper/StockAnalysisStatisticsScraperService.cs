using HtmlAgilityPack;
using FinanceScraper.Common.Base;
using FinanceScraper.Common.DataSets;
using FinanceScraper.Common.Exceptions.ExceptionResolver;
using FinanceScraper.Common.Extensions;
using FinanceScraper.StockAnalysis.StatisticsScraper.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScraper.StockAnalysis.StatisticsScraper
{
    public class StockAnalysisStatisticsScraperService : IScrapeServiceStrategy<StatisticsScraperCommand, StatisticsDataSet>
    {
        private readonly IExceptionResolverService _exceptionResolverService;
        public StockAnalysisStatisticsScraperService(IExceptionResolverService exceptionResolverService)
        {
            _exceptionResolverService = exceptionResolverService;
        }
        public async Task<StatisticsDataSet> ExecuteScrape(StatisticsScraperCommand request)
        {
            HtmlNode node = await request.NodeResolverAsync();

            //sync because only single scrape is executed

            decimal sharesOutstanding = GetSharesOutstanding(node, request.Ticker);

            return new StatisticsDataSet()
            {
                SharesOutstanding = sharesOutstanding,
            };
        }

        private decimal GetSharesOutstanding(HtmlNode node, string ticker)
        {
            string commonExceptionSuffix = string.Format(" while attempting to acquire Outstanding Shares for the following ticker: {0}.", ticker);

            HtmlNode sharesOutstandingNode =  node.SelectSingleNode("//td/span[contains(., 'Shares Outstanding')]/parent::td/parent::tr/td[2]");

            _exceptionResolverService.HtmlNodeNullReferenceExceptionResolver(sharesOutstandingNode, commonExceptionSuffix);

            string toConvert = sharesOutstandingNode.InnerHtml.Split('M')[0];

            decimal sharesOutstanding = _exceptionResolverService.ConvertToDecimalExceptionResolver(toConvert, commonExceptionSuffix);

            return sharesOutstanding * 1000000;
        }
    }
}
