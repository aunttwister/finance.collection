using HtmlAgilityPack;
using FinanceScraper.Common.Base;
using FinanceScraper.Common.DataSets;
using FinanceScraper.Common.Exceptions;
using FinanceScraper.Common.Exceptions.ExceptionResolver;
using FinanceScraper.Common.Extensions;
using FinanceScraper.YahooFinance.CashFlowScraper.Commands;

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
            HtmlNode node = await request.NodeResolverAsync();

            Dictionary<string, decimal> historicalYearCashFlows = await GetHistoricalYearCashFlow(node, request.Ticker);

            return new CashFlowDataSet() { HistoricalYearCashFlows =  historicalYearCashFlows };
        }

        private async Task<Dictionary<string, decimal>> GetHistoricalYearCashFlow(HtmlNode node, string ticker)
        {
            string commonExceptionSuffix = string.Format(" while attempting to acquire Historical Cash Flow for Year : CashFlow Dictionary for the following ticker: {0}.", ticker);

            Task<IEnumerable<string>> taskYears = Task.Run(() => ResolveYears(node, commonExceptionSuffix));

            Task<IEnumerable<decimal>> taskCashFlows = Task.Run(() => ResolveCashFlow(node, commonExceptionSuffix));

            //await
            await Task.WhenAll(taskYears, taskCashFlows);

            return taskYears.Result.Zip(taskCashFlows.Result, (k, v) => new { k, v })
                        .ToDictionary(x => x.k, x => x.v / 100);
        }

        public IEnumerable<string> ResolveYears(HtmlNode node, string commonExceptionSuffix)
        {
            HtmlNodeCollection yearNodeCollection = node.SelectNodes("//div/span[text()=\"Breakdown\"]/parent::div/parent::div/div/span");

            _exceptionResolverService.HtmlNodeCollectionNullReferenceExceptionResolver(yearNodeCollection, commonExceptionSuffix);

            return yearNodeCollection.Nodes()
                   .Where(node => node.InnerHtml != "Breakdown")
                   .Select(node => node.InnerHtml);
        }

        public async Task<IEnumerable<decimal>> ResolveCashFlow(HtmlNode node, string commonExceptionSuffix)
        {
            HtmlNodeCollection cashFlowNodeCollection = await Task.Run(() => node.SelectNodes("//div/span[text()=\"Free Cash Flow\"]/parent::div/parent::div/parent::div/div/span"));

            _exceptionResolverService.HtmlNodeCollectionNullReferenceExceptionResolver(cashFlowNodeCollection, commonExceptionSuffix);

            IEnumerable<string> cashFlowScraped = cashFlowNodeCollection.Nodes()
                                                .Select(node => node.InnerHtml);

            List<decimal> cashFlows = new List<decimal>();

            foreach (string toConvert in cashFlowScraped)
            {
                decimal cashFlow = _exceptionResolverService.ConvertToDecimalExceptionResolver(toConvert, commonExceptionSuffix);
                cashFlows.Add(cashFlow);
            }

            return cashFlows;
        }
    }
}
