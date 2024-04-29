using FinanceScraper.Common.Base;
using FinanceScraper.Common.Exceptions.ExceptionResolver;
using FinanceScraper.Common.Extensions;
using FinanceScraper.StockAnalysis.BalanceSheetScraper.Commands;
using FinanceScraper.StockAnalysis.CashFlowScraper.Commands;
using HtmlAgilityPack;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.FinanceScraper.DataSets;

namespace FinanceScraper.StockAnalysis.CashFlowScraper
{
    public class StockAnalysisCashFlowScrapeService : IScrapeServiceStrategy<StockAnalysisCashFlowScraperCommand, CashFlowDataSet>
    {
        private readonly IExceptionResolverService _exceptionResolverService;
        public StockAnalysisCashFlowScrapeService(IExceptionResolverService exceptionResolverService)
        {
            _exceptionResolverService = exceptionResolverService;
        }
        public async Task<CashFlowDataSet> ExecuteScrape(StockAnalysisCashFlowScraperCommand request)
        {
            HtmlNode node = await request.NodeResolverAsync().ConfigureAwait(false);

            Task<MethodResultDictionary<string, decimal>> historicalYearCashFlows = GetHistoricalYearCashFlow(node);

            await historicalYearCashFlows.ConfigureAwait(false);

            return new CashFlowDataSet()
            {
                HistoricalCashFlows = historicalYearCashFlows.Result
            };
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
                                                                       .ToDictionary(x => x.k, x => x.v);

                return new MethodResultDictionary<string, decimal>(dictionary, exceptionPair);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public MethodResult<IEnumerable<string>> ResolveYears(HtmlNode node)
        {
            HtmlNodeCollection nodeCollection = node.SelectNodes("//th[text()=\"Year\"]/following-sibling::th");

            Func<MethodResult<IEnumerable<string>>>[] operations = new Func<MethodResult<IEnumerable<string>>>[]
            {
                () => _exceptionResolverService.HtmlNodeCollectionNullReferenceExceptionResolver<IEnumerable<string>>(nodeCollection)
            };

            MethodResult<IEnumerable<string>> result = node.ExecuteUntilFirstException(operations);

            if (!result.IsSuccessful)
                return result;

            IEnumerable<string> data = nodeCollection.Nodes()
                                            .Where(node => !node.InnerHtml.Contains('-'))
                                            .Select(node => node.InnerHtml);
            result.AssignData(data);

            return result;
        }

        public MethodResult<IEnumerable<decimal>> ResolveCashFlow(HtmlNode node)
        {
            HtmlNodeCollection nodeCollection = node.SelectNodes("//td/span[text()=\"Free Cash Flow\"]/parent::td/following-sibling::td");

            Func<MethodResult<IEnumerable<decimal>>>[] operations = new Func<MethodResult<IEnumerable<decimal>>>[]
            {
                () => _exceptionResolverService.HtmlNodeCollectionNullReferenceExceptionResolver<IEnumerable<decimal>>(nodeCollection),
                () => _exceptionResolverService.MultiConvertToDecimalExceptionResolver(nodeCollection.Nodes().Where(node => !node.InnerHtml.Contains("Upgrade") && !node.InnerHtml.Contains(' ')).Select(node => node.InnerHtml))
            };

            MethodResult<IEnumerable<decimal>> result = nodeCollection.ExecuteUntilFirstException(operations);

            if (!result.IsSuccessful) 
                return result;

            List<decimal> updatedValue = new List<decimal>();

            foreach (decimal value in result.Data)
            {
                decimal newValue = value * 1000000;
                updatedValue.Add(newValue);
            }

            result.AssignData(updatedValue);

            return result;
        }
    }
}
