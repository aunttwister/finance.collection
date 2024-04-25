using FinanceScraper.YahooFinance.SummaryScraper.Commands;
using FinanceScraper.YCharts.TripleABondYieldScraper.Commands;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceScraper.Common.Extensions;
using FinanceScraper.Common.Exceptions.ExceptionResolver;
using FinanceScraper.Common.Base;
using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.FinanceScraper.DataSets;

namespace FinanceScraper.YCharts.TripleABondYieldScraper
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
            HtmlNode node = await request.NodeResolverAsync().ConfigureAwait(false);

            Task<MethodResult<decimal>> currentTripleABond = Task.Run(() => GetCurrentBondTripleAYield(node));

            Task<MethodResult<decimal>> historicalAverageTripleABond = Task.Run(() => GetHistoricalAverageBondTripleAYield(node));

            await Task.WhenAll(currentTripleABond, historicalAverageTripleABond).ConfigureAwait(false);

            return new TripleABondsDataSet() { 
                CurrentTripleABond = currentTripleABond.Result, 
                HistoricalAverageTripleABond = historicalAverageTripleABond.Result 
            };
        }

        [HandleMethodExecutionAspect]
        private Task<MethodResult<decimal>> GetCurrentBondTripleAYield(HtmlNode node)
        {
            try
            {
                return Task.Run(() => ResolveCurrentBondTripleAYield(node));
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HandleMethodExecutionAspect]
        private Task<MethodResult<decimal>> GetHistoricalAverageBondTripleAYield(HtmlNode node)
        {
            try
            {
                return Task.Run(() => ResolveAverageBondTripleAYield(node));
            }
            catch (Exception) 
            {
                throw; 
            }
        }

        public MethodResult<decimal> ResolveCurrentBondTripleAYield(HtmlNode node)
        {
            node = node.SelectSingleNode("//td[text()='Last Value']/following-sibling::td[1]");

            char splitChar = '%';

            Func<MethodResult<decimal>>[] operations = new Func<MethodResult<decimal>>[]
            {
                () => _exceptionResolverService.HtmlNodeNullReferenceExceptionResolver<decimal>(node),
                () => _exceptionResolverService.HtmlNodeNotApplicableExceptionResolver<decimal>(node),
                () => _exceptionResolverService.HtmlNodeKeyCharacterNotFoundExceptionResolver<decimal>(node, splitChar),
                () => _exceptionResolverService.ConvertToDecimalExceptionResolver(node.InnerHtml.Split(splitChar)[0])
            };

            MethodResult<decimal> result = node.ExecuteUntilFirstException(operations);

            if (!result.IsSuccessful)
                return result;

            decimal data = result.Data / 100;

            result.AssignData(data);

            return result;
        }

        public MethodResult<decimal> ResolveAverageBondTripleAYield(HtmlNode node)
        {
            node = node.SelectSingleNode("//td[text()='Long Term Average']/following-sibling::td[1]");

            char splitChar = '%';

            Func<MethodResult<decimal>>[] operations = new Func<MethodResult<decimal>>[]
            {
                () => _exceptionResolverService.HtmlNodeNullReferenceExceptionResolver<decimal>(node),
                () => _exceptionResolverService.HtmlNodeNotApplicableExceptionResolver<decimal>(node),
                () => _exceptionResolverService.HtmlNodeKeyCharacterNotFoundExceptionResolver<decimal>(node, splitChar),
                () => _exceptionResolverService.ConvertToDecimalExceptionResolver(node.InnerHtml.Split(splitChar)[0])
            };

            MethodResult<decimal> result = node.ExecuteUntilFirstException(operations);

            if (!result.IsSuccessful)
                return result;

            decimal data = result.Data / 100;

            result.AssignData(data);

            return result;
        }
    }
}
