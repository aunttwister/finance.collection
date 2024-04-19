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
using FinanceScraper.Common.Propagation;

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
            HtmlNode node = await request.NodeResolverAsync().ConfigureAwait(false);

            Task<MethodResult<decimal>> sharesOutstanding = Task.Run(() => GetSharesOutstanding(node));

            await sharesOutstanding.ConfigureAwait(false);

            return new StatisticsDataSet()
            {
                SharesOutstanding = sharesOutstanding.Result,
            };
        }

        [HandleMethodExecutionAspect]
        private Task<MethodResult<decimal>> GetSharesOutstanding(HtmlNode node)
        {
            try
            {
                return Task.Run(() => ResolveSharesOutstanding(node));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public MethodResult<decimal> ResolveSharesOutstanding(HtmlNode node)
        {
            node = node.SelectSingleNode("//td/span[contains(., 'Shares Outstanding')]/parent::td/parent::tr/td[2]");

            char splitChar = 'M';

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
