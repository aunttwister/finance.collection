using MediatR;
using FinanceScraper.Common.Base;
using Finance.Collection.Domain.FinanceScraper.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScraper.StockAnalysis.BalanceSheetScraper.Commands
{
    public class StockAnalysisBalanceSheetScraperCommand : StockAnalysisScraperBaseCommand, IRequest<BalanceSheetDataSet>
    {
        public StockAnalysisBalanceSheetScraperCommand(string ticker, string path) : base(ticker, path) { }
    }
}
