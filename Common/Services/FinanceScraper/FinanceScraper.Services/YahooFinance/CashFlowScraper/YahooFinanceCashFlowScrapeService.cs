using HtmlAgilityPack;
using FinanceScraper.Common.Base;
using FinanceScraper.Common.Exceptions;
using FinanceScraper.Common.Exceptions.ExceptionResolver;
using FinanceScraper.Common.Extensions;
using FinanceScraper.YahooFinance.CashFlowScraper.Commands;
using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.FinanceScraper.DataSets;

namespace FinanceScraper.YahooFinance.CashFlowScraper
{
    public class YahooFinanceCashFlowScrapeService : IScrapeServiceStrategy<YahooFinanceCashFlowScraperCommand, CashFlowDataSet>
    {
        private readonly IExceptionResolverService _exceptionResolverService;
        public YahooFinanceCashFlowScrapeService(IExceptionResolverService exceptionResolverService)
        {
            _exceptionResolverService = exceptionResolverService;
        }
        public async Task<CashFlowDataSet> ExecuteScrape(YahooFinanceCashFlowScraperCommand request)
        {
            HtmlNode node = await request.NodeResolverAsync().ConfigureAwait(false);

            Task<MethodResultDictionary<string, decimal>> taskHistoricalCashFlows = GetHistoricalYearCashFlow(node);

            await taskHistoricalCashFlows.ConfigureAwait(false);

            return new CashFlowDataSet() { HistoricalCashFlows = taskHistoricalCashFlows.Result };
        }

        [HandleMethodExecutionAspect]
        private async Task<MethodResultDictionary<string, decimal>> GetHistoricalYearCashFlow(HtmlNode node)
        {
            try
            {
                Task<MethodResult<IEnumerable<string>>> taskYears = Task.Run(() => ResolveYears(node));

                Task<MethodResult<IEnumerable<decimal>>> taskCashFlows = Task.Run(() => ResolveCashFlow(node));

                //await
                await Task.WhenAll(taskYears, taskCashFlows).ConfigureAwait(false);

                KeyValuePair<Exception, Exception> exceptionPair = new KeyValuePair<Exception, Exception>(taskYears.Result.Exception, taskCashFlows.Result.Exception);

                if (!taskYears.Result.IsSuccessful || !taskCashFlows.Result.IsSuccessful)
                    return new MethodResultDictionary<string, decimal>(null, exceptionPair);

                Dictionary<string, decimal> dictionary = taskYears.Result.Data.Zip(taskCashFlows.Result.Data, (k, v) => new { k, v })
                                                                       .ToDictionary(x => x.k, x => x.v / 100);

                return new MethodResultDictionary<string, decimal>(dictionary, exceptionPair);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public MethodResult<IEnumerable<string>> ResolveYears(HtmlNode node)
        {
            HtmlNodeCollection nodeCollection = node.SelectNodes("//div/span[text()=\"Breakdown\"]/parent::div/parent::div/div/span");

            Func<MethodResult<IEnumerable<string>>>[] operations = new Func<MethodResult<IEnumerable<string>>>[]
            {
                () => _exceptionResolverService.HtmlNodeCollectionNullReferenceExceptionResolver<IEnumerable<string>>(nodeCollection)
            };

            MethodResult<IEnumerable<string>> result = node.ExecuteUntilFirstException(operations);

            if (!result.IsSuccessful)
                return result;

            IEnumerable<string> data = nodeCollection.Nodes()
                                            .Where(node => node.InnerHtml != "Breakdown")
                                            .Select(node => node.InnerHtml);
            result.AssignData(data);

            return result;
        }

        public MethodResult<IEnumerable<decimal>> ResolveCashFlow(HtmlNode node)
        {
            HtmlNodeCollection nodeCollection = node.SelectNodes("//div/span[text()=\"Free Cash Flow\"]/parent::div/parent::div/parent::div/div/span");

            Func<MethodResult<IEnumerable<decimal>>> [] operations = new Func<MethodResult<IEnumerable<decimal>>>[]
            {
                () => _exceptionResolverService.HtmlNodeCollectionNullReferenceExceptionResolver<IEnumerable<decimal>>(nodeCollection),
                () => _exceptionResolverService.MultiConvertToDecimalExceptionResolver(nodeCollection.Nodes().Select(node => node.InnerHtml))
            };

            return nodeCollection.ExecuteUntilFirstException(operations);
        }
    }
}
