using Finance.Collection.Domain.FinanceScraper.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Collection.Domain.FinanceScraper.Results
{
    public class DCFScrapeResult : IScrapeResult, ICalculable
    {
        public string Ticker { get; set; }
        public CashFlowDataSet CashFlow { get; set; }
        public BalanceSheetDataSet BalanceSheet { get; set; }
        public StatisticsDataSet Statistics { get; set; }
    }
}
