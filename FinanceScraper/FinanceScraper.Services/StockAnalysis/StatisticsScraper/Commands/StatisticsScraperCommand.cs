using MediatR;
using StockPortfolio.FinanceScraper.Common.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPortfolio.FinanceScraper.StockAnalysis.StatisticsScraper.Commands
{
    public class StatisticsScraperCommand : StockAnalysisScraperBaseCommand, IRequest<StatisticsDataSet>
    {
        public StatisticsScraperCommand(string ticker, string path) : base (ticker, path) { }
    }
}
