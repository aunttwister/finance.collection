using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finance.Collection.Domain.FinanceScraper.DataSets;

namespace FinanceScraper.YahooFinance.AnalysisScraper.Commands
{
    public class AnalysisScraperCommand : YahooFinanceScraperBaseCommand, IRequest<AnalysisDataSet>

    {
        public AnalysisScraperCommand(string ticker, string path) : base(ticker, path) { }
    }
}
