using MediatR;
using StockPortfolio.FinanceScraper.Common.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPortfolio.FinanceScraper.YahooFinance.CashFlowScraper.Commands
{
    public class YahooFinanceCashFlowScraperCommand : YahooFinanceScraperBaseCommand, IRequest<CashFlowDataSet>

    {
        public YahooFinanceCashFlowScraperCommand(string ticker, string path) : base(ticker, path) { }
    }
}
