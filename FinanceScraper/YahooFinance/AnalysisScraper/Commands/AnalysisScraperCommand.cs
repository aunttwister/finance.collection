using MediatR;
using StockPortfolio.FinanceScraper.Common.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPortfolio.FinanceScraper.YahooFinance.AnalysisScraper.Commands
{
    public class AnalysisScraperCommand : YahooFinanceScraperBaseCommand, IRequest<AnalysisDataSet>

    {
        public AnalysisScraperCommand(string ticker, string path) : base(ticker, path) { }
    }
}
