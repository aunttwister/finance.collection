using FinanceScraper.YahooFinance;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceScraper.MacroTrends;
using FinanceScraper.Common.DataSets;

namespace FinanceScraper.MacroTrends.CashFlow.Commands
{
    public class MacroTrendsCashOnHandScraperCommand : MacroTrendsScraperBaseCommand, IRequest<CashFlowDataSet>

    {
        public MacroTrendsCashOnHandScraperCommand(string ticker, string path, string actionPath) : base(ticker, path, actionPath) { }
    }
}
