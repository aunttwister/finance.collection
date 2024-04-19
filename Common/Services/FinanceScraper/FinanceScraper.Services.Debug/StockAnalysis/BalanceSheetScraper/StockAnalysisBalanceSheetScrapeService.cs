using HtmlAgilityPack;
using FinanceScraper.Common.Base;
using FinanceScraper.Common.DataSets;
using FinanceScraper.Common.Exceptions.ExceptionResolver;
using FinanceScraper.Common.Extensions;
using FinanceScraper.StockAnalysis.BalanceSheetScraper.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceScraper.Common.Propagation;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinanceScraper.StockAnalysis.BalanceSheetScraper
{
    public class StockAnalysisBalanceSheetScrapeService : IScrapeServiceStrategy<BalanceSheetScraperCommand, BalanceSheetDataSet>
    {
        private readonly IExceptionResolverService _exceptionResolverService;
        public StockAnalysisBalanceSheetScrapeService(IExceptionResolverService exceptionResolverService)
        {
            _exceptionResolverService = exceptionResolverService;
        }
        public async Task<BalanceSheetDataSet> ExecuteScrape(BalanceSheetScraperCommand request)
        {
            HtmlNode node = await request.NodeResolverAsync().ConfigureAwait(false);

            Task<Dictionary<string, decimal>> HistoricalCashEquivalents = GetHistoricalCashEquivalents(node);

            Task<Dictionary<string, decimal>> HistoricalTotalDebt = GetHistoricalTotalDebt(node);

            await Task.WhenAll(HistoricalCashEquivalents, HistoricalTotalDebt).ConfigureAwait(false);

            decimal ttmCashEquivalents = HistoricalCashEquivalents.Result.First(cashEquivalents => cashEquivalents.Key.Equals("2023")).Value;
            decimal ttmTotalDebt = HistoricalTotalDebt.Result.First(cashEquivalents => cashEquivalents.Key.Equals("2023")).Value;

            return new BalanceSheetDataSet()
            {
                TTMCashEquivalents = ttmCashEquivalents,
                TTMTotalDebt = ttmTotalDebt,
                HistoricalCashEquivalents = HistoricalCashEquivalents.Result,
                HistoricalTotalDebt = HistoricalTotalDebt.Result
            };
        }

        [HandleMethodExecutionAspect]
        private async Task<Dictionary<string, decimal>> GetHistoricalCashEquivalents(HtmlNode node)
        {
            Task<MethodResult<IEnumerable<string>>> taskYears = Task.Run(() => ResolveYears(node));

            Task<MethodResult<IEnumerable<decimal>>> taskCashEquivalents = Task.Run(() => ResolveCashEquivalents(node));

            await Task.WhenAll(taskYears, taskCashEquivalents).ConfigureAwait(false);

            return taskYears.Result.Data.Zip(taskCashEquivalents.Result.Data, (k, v) => new { k, v })
                .ToDictionary(x => x.k, x => x.v);
        }

        [HandleMethodExecutionAspect]
        public async Task<Dictionary<string, decimal>> GetHistoricalTotalDebt(HtmlNode node)
        {

            Task<MethodResult<IEnumerable<string>>> taskYears = Task.Run(() => ResolveYears(node));

            Task<MethodResult<IEnumerable<decimal>>> taskTotalDebt = Task.Run(() => ResolveTotalDebt(node));

            await Task.WhenAll(taskYears, taskTotalDebt).ConfigureAwait(false);

            return taskYears.Result.Data.Zip(taskTotalDebt.Result.Data, (k, v) => new { k, v })
                .ToDictionary(x => x.k, x => x.v);
        }

        public MethodResult<IEnumerable<string>> ResolveYears(HtmlNode node)
        {
            HtmlNodeCollection nodeCollection = node.SelectNodes("//tr/th[text()=\"Year\"]/parent::tr/th");

            Func<MethodResult<IEnumerable<string>>>[] operations = new Func<MethodResult<IEnumerable<string>>>[]
            {
                () => _exceptionResolverService.HtmlNodeCollectionNullReferenceExceptionResolver<IEnumerable<string>>(nodeCollection)
            };

            MethodResult<IEnumerable<string>> result = node.ExecuteUntilFirstException(operations);

            if (!result.IsSuccessful)
                return result;

            IEnumerable<string> data = nodeCollection.Nodes()
                                            .Where(node => !node.InnerHtml.Contains('-') && !node.InnerHtml.Equals("Year"))
                                            .Select(node => node.InnerHtml);

            result.AssignData(data);

            return result;
        }

        public MethodResult<IEnumerable<decimal>> ResolveCashEquivalents(HtmlNode node)
        {
            HtmlNodeCollection nodeCollection = node.SelectNodes("//td/span[contains(.,' Cash Equivalents')]/parent::td/parent::tr/td");

            Func<MethodResult<IEnumerable<decimal>>>[] operations = new Func<MethodResult<IEnumerable<decimal>>>[]
            {
                () => _exceptionResolverService.HtmlNodeNullReferenceExceptionResolver<IEnumerable<decimal>>(node),
                () => _exceptionResolverService.MultiConvertToDecimalExceptionResolver(nodeCollection.Nodes()
                                               .Where(node => !node.InnerHtml.Contains("Upgrade") && !node.InnerHtml.Contains(' ') && !node.InnerHtml.Contains("HTML"))
                                               .Select(node => node.InnerHtml))
            };

            MethodResult <IEnumerable<decimal>> result = nodeCollection.ExecuteUntilFirstException(operations);

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

        public MethodResult<IEnumerable<decimal>> ResolveTotalDebt(HtmlNode node)
        {
            HtmlNodeCollection nodeCollection = node.SelectNodes("//td/span[contains(.,'Total Debt')]/parent::td/parent::tr/td");

            Func<MethodResult<IEnumerable<decimal>>>[] operations = new Func<MethodResult<IEnumerable<decimal>>>[]
            {
                () => _exceptionResolverService.HtmlNodeNullReferenceExceptionResolver<IEnumerable<decimal>>(node),
                () => _exceptionResolverService.MultiConvertToDecimalExceptionResolver(nodeCollection.Nodes()
                                               .Where(node => !node.InnerHtml.Contains("Upgrade") && !node.InnerHtml.Contains(' ') && !node.InnerHtml.Contains("HTML"))
                                               .Select(node => node.InnerHtml))
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
