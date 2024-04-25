using MediatR;
using FinanceScraper.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finance.Collection.Domain.FinanceScraper.DataSets;

namespace FinanceScraper.StockAnalysis.BalanceSheetScraper.Commands
{
    public class StockAnalysisBalanceSheetScraperCommand : StockAnalysisScraperBaseCommand, IRequest<BalanceSheetDataSet>
    {
        public StockAnalysisBalanceSheetScraperCommand(string ticker, string path) : base(ticker, path) { }
    }
}
