using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finance.Collection.Domain.FinanceScraper.DataSets;

namespace FinanceScraper.StockAnalysis.StatisticsScraper.Commands
{
    public class StockAnalysisStatisticsScraperCommand : StockAnalysisScraperBaseCommand, IRequest<StatisticsDataSet>
    {
        public StockAnalysisStatisticsScraperCommand(string ticker, string path) : base (ticker, path) { }
    }
}
