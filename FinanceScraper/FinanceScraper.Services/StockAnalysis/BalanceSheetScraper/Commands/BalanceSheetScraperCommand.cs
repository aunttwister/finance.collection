using MediatR;
using StockPortfolio.FinanceScraper.Common.Base;
using StockPortfolio.FinanceScraper.Common.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPortfolio.FinanceScraper.StockAnalysis.BalanceSheetScraper.Commands
{
    public class BalanceSheetScraperCommand : StockAnalysisScraperBaseCommand, IRequest<BalanceSheetDataSet>
    {
        public BalanceSheetScraperCommand(string ticker, string path) : base(ticker, path) { }
    }
}
