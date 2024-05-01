using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finance.Collection.Domain.FinanceScraper.DataSets;
using FinanceScraper.Common.NodeResolver;

namespace FinanceScraper.YahooFinance.CashFlowScraper.Commands
{
    public class YahooFinanceCashFlowScraperCommand : YahooFinanceScraperBaseCommand, IRequest<CashFlowDataSet>

    {
        public YahooFinanceCashFlowScraperCommand(string ticker, string path) : base(ticker, path) { }
    }
}
