using StockPortfolio.FinanceScraper.Common.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPortfolio.FinanceScraper.Executable
{
    public struct ScraperDataSetStruct
    {
        public TickerDataSet TickerDataSet { get; set; }
        public TripleABondsDataSet TripleABondsDataSet { get; set;}
    }
}
