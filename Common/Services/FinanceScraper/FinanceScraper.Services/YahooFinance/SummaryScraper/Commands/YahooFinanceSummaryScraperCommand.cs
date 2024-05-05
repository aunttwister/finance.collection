using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finance.Collection.Domain.FinanceScraper.DataSets;
using FinanceScraper.Common.NodeResolver;

namespace FinanceScraper.YahooFinance.SummaryScraper.Commands
{
    public class YahooFinanceSummaryScraperCommand : YahooFinanceScraperBaseCommand, IRequest<SummaryDataSet>

    {
        public YahooFinanceSummaryScraperCommand(string ticker, string path) : base(ticker, path) { }
    }
}
