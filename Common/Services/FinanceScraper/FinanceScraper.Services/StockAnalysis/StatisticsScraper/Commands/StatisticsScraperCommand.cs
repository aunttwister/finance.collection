using MediatR;
using FinanceScraper.Common.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScraper.StockAnalysis.StatisticsScraper.Commands
{
    public class StatisticsScraperCommand : StockAnalysisScraperBaseCommand, IRequest<StatisticsDataSet>
    {
        public StatisticsScraperCommand(string ticker, string path) : base (ticker, path) { }
    }
}
