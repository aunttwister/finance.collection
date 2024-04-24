using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScraper.Common.DataSets.Results
{
    public class DCFIntrinsicScrapeResult : IScrapeResult
    {
        public CashFlowDataSet CashFlow { get; set; }
        public BalanceSheetDataSet BalanceSheet { get; set; }
        public StatisticsDataSet Statistics { get; set; }
    }
}
