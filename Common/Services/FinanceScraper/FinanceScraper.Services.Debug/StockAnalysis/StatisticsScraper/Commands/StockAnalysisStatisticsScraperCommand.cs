using MediatR;
using FinanceScraper.Common.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScraper.StockAnalysis.StatisticsScraper.Commands
{
    public class StockAnalysisStatisticsScraperCommand : StockAnalysisScraperBaseCommand, IRequest<StatisticsDataSet>
    {
        public StockAnalysisStatisticsScraperCommand(string ticker, string path) : base (ticker, path) { }
    }
}
