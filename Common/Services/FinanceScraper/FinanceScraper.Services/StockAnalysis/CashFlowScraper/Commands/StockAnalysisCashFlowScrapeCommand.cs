using Finance.Collection.Domain.FinanceScraper.DataSets;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScraper.StockAnalysis.CashFlowScraper.Commands
{
    public class StockAnalysisCashFlowScraperCommand : StockAnalysisScraperBaseCommand, IRequest<CashFlowDataSet>
    {
        public StockAnalysisCashFlowScraperCommand(string ticker, string path) : base(ticker, path) { }
    }
}
