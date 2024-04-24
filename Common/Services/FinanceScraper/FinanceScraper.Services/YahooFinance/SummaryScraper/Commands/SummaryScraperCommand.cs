using MediatR;
using Finance.Collection.Domain.FinanceScraper.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScraper.YahooFinance.SummaryScraper.Commands
{
    public class SummaryScraperCommand : YahooFinanceScraperBaseCommand, IRequest<SummaryDataSet>

    {
        public SummaryScraperCommand(string ticker, string path) : base(ticker, path) { }
    }
}
