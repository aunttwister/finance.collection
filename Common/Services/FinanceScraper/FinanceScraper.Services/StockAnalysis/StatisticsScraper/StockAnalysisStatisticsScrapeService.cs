using HtmlAgilityPack;
using FinanceScraper.Common.Base;
using FinanceScraper.Common.Exceptions.ExceptionResolver;
using FinanceScraper.Common.Extensions;
using FinanceScraper.StockAnalysis.StatisticsScraper.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.Common.CustomDataType;
using Finance.Collection.Domain.FinanceScraper.DataSets;

namespace FinanceScraper.StockAnalysis.StatisticsScraper
{
    public class StockAnalysisStatisticsScrapeService : IScrapeServiceStrategy<StockAnalysisStatisticsScraperCommand, StatisticsDataSet>
    {
        private readonly IExceptionResolverService _exceptionResolverService;
        public StockAnalysisStatisticsScrapeService(IExceptionResolverService exceptionResolverService)
        {
            _exceptionResolverService = exceptionResolverService;
        }
        public async Task<StatisticsDataSet> ExecuteScrape(StockAnalysisStatisticsScraperCommand request)
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

            Func<MethodResult<decimal>>[] operations = new Func<MethodResult<decimal>>[]
            {
                () => _exceptionResolverService.HtmlNodeNullReferenceExceptionResolver<decimal>(node),
                () => _exceptionResolverService.HtmlNodeNotApplicableExceptionResolver<decimal>(node)
            };

            MethodResult<decimal> result = node.ExecuteUntilFirstException(operations);

            if (!result.IsSuccessful)
                return result;

            LargeNumber largeNumber = new LargeNumber()
            {
                SplitterDenominatorPair = new KeyValuePair<char, int>('M', 1000000)
            };

            switch (node.InnerHtml)
            {
                case string value2 when node.InnerHtml.ToLower().Contains('b'):
                    largeNumber.SplitterDenominatorPair = new KeyValuePair<char, int>('B', 1000000000);
                    break;
                default:
                    break;
            }

            operations = new Func<MethodResult<decimal>>[]
            {
                () => _exceptionResolverService.HtmlNodeKeyCharacterNotFoundExceptionResolver<decimal>(node, largeNumber.SplitterDenominatorPair.Key),
                () => _exceptionResolverService.ConvertToDecimalExceptionResolver(node.InnerHtml.Split(largeNumber.SplitterDenominatorPair.Key)[0])
            };

            result = node.ExecuteUntilFirstException(operations);

            if (!result.IsSuccessful)
                return result;

            result.AssignData(result.Data * largeNumber.SplitterDenominatorPair.Value);

            return result;
        }
    }
}
