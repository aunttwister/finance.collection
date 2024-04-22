using FinanceScraper.Common.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPortfolio.FinanceScraper.Executable
{
    public class CalculationResult
    {
        public TickerDataSet TickerDataSet { get; set; }
        public TripleABondsDataSet TripleABondsDataSet { get; set;}
    }
}
