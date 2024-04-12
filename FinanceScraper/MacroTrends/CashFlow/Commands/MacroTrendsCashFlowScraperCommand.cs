using StockPortfolio.FinanceScraper.YahooFinance;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockPortfolio.FinanceScraper.MacroTrends;
using StockPortfolio.FinanceScraper.Common.DataSets;

namespace StockPortfolio.FinanceScraper.MacroTrends.CashFlow.Commands
{
    public class MacroTrendsCashFlowScraperCommand : MacroTrendsScraperBaseCommand, IRequest<CashFlowDataSet>

    {
        public MacroTrendsCashFlowScraperCommand(string ticker, string basePath, string actionPath) : base(ticker, basePath, actionPath) 
        {
            ResolveUrl();
        }
    }
}
