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
            HtmlNode node = await request.NodeResolverAsync();

            Task<Dictionary<string, decimal>> HistoricalCashEquivalents = GetHistoricalCashEquivalents(node, request.Ticker);

            Task<Dictionary<string, decimal>> HistoricalTotalDebt = GetHistoricalTotalDebt(node, request.Ticker);

            await Task.WhenAll(HistoricalCashEquivalents, HistoricalTotalDebt);

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

        private async Task<Dictionary<string, decimal>> GetHistoricalCashEquivalents(HtmlNode node, string ticker)
        {
            string commonExceptionSuffix = string.Format(" while attempting to acquire Historical Cash Equivalents for Year : CashEquivalents Dictionary for the following ticker: {0}.", ticker);

            Task<IEnumerable<string>> taskYears = Task.Run(() => ResolveYears(node, commonExceptionSuffix));

            Task<IEnumerable<decimal>> taskCashEquivalents = Task.Run(() => ResolveCashEquivalents(node, commonExceptionSuffix));

            await Task.WhenAll(taskYears, taskCashEquivalents);

            return taskYears.Result.Zip(taskCashEquivalents.Result, (k, v) => new { k, v })
                .ToDictionary(x => x.k, x => x.v);
        }

        public async Task<Dictionary<string, decimal>> GetHistoricalTotalDebt(HtmlNode node, string ticker)
        {
            string commonExceptionSuffix = string.Format(" while attempting to acquire Historical Total Debt for Year : TotalDebt Dictionary for the following ticker: {0}.", ticker);

            Task<IEnumerable<string>> taskYears = Task.Run(() => ResolveYears(node, commonExceptionSuffix));

            Task<IEnumerable<decimal>> taskTotalDebt = Task.Run(() => ResolveTotalDebt(node, commonExceptionSuffix));

            await Task.WhenAll(taskYears, taskTotalDebt);

            return taskYears.Result.Zip(taskTotalDebt.Result, (k, v) => new { k, v })
                .ToDictionary(x => x.k, x => x.v);
        }

        public IEnumerable<string> ResolveYears(HtmlNode node, string commonExceptionSuffix)
        {
            HtmlNodeCollection yearNodeCollection = node.SelectNodes("//tr/th[text()=\"Year\"]/parent::tr/th");

            _exceptionResolverService.HtmlNodeCollectionNullReferenceExceptionResolver(yearNodeCollection, commonExceptionSuffix);

            return yearNodeCollection.Nodes()
                   .Where(node => !node.InnerHtml.Contains('-') && !node.InnerHtml.Equals("Year"))
                   .Select(node => node.InnerHtml);
        }

        public async Task<IEnumerable<decimal>> ResolveCashEquivalents(HtmlNode node, string commonExceptionSuffix)
        {
            HtmlNodeCollection cashEquivalentsNodeCollection = await Task.Run(() => node.SelectNodes("//td/span[contains(.,' Cash Equivalents')]/parent::td/parent::tr/td"));

            _exceptionResolverService.HtmlNodeCollectionNullReferenceExceptionResolver(cashEquivalentsNodeCollection, commonExceptionSuffix);

            IEnumerable<string> cashEquivalentsScraped = cashEquivalentsNodeCollection.Nodes()
                                                .Where(node => !node.InnerHtml.Contains("Upgrade") && !node.InnerHtml.Contains(' ') && !node.InnerHtml.Contains("HTML"))
                                                .Select(node => node.InnerHtml);

            List<decimal> cashEquivalents = new List<decimal>();

            foreach (string toConvert in cashEquivalentsScraped)
            {
                decimal cashEquivalent = _exceptionResolverService.ConvertToDecimalExceptionResolver(toConvert, commonExceptionSuffix);
                cashEquivalent *= 1000000;
                cashEquivalents.Add(cashEquivalent);
            }

            return cashEquivalents;
        }

        public async Task<IEnumerable<decimal>> ResolveTotalDebt(HtmlNode node, string commonExceptionSuffix)
        {
            HtmlNodeCollection totalDebtNodeCollection = await Task.Run(() => node.SelectNodes("//td/span[contains(.,'Total Debt')]/parent::td/parent::tr/td"));

            _exceptionResolverService.HtmlNodeCollectionNullReferenceExceptionResolver(totalDebtNodeCollection, commonExceptionSuffix);

            IEnumerable<string> totalDebtScraped = totalDebtNodeCollection.Nodes()
                                                .Where(node => !node.InnerHtml.Contains("Upgrade") && !node.InnerHtml.Contains(' ') && !node.InnerHtml.Contains("HTML"))
                                                .Select(node => node.InnerHtml);

            List<decimal> totalDebts = new List<decimal>();

            foreach (string toConvert in totalDebtScraped)
            {
                decimal totalDebt = _exceptionResolverService.ConvertToDecimalExceptionResolver(toConvert, commonExceptionSuffix);
                totalDebt *= 1000000;
                totalDebts.Add(totalDebt);
            }

            return totalDebts;
        }
    }
}
