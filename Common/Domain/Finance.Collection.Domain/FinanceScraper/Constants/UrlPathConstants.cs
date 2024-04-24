using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Collection.Domain.FinanceScraper.Constants
{
    public static class UrlPathConstants
    {
        public const string YahooFinanceSummaryScraperPath = "?p=";
        public const string YahooFinanceAnalysisScraperPath = "/analysis?p=";
        public const string YahooFinanceCashFlowScraperPath = "/cash-flow?p=";
        public const string MacroTrendsBaseScraperPath = "/stocks/charts/";
        public const string MacroTrendsCashFlowScraperPath = "/free-cash-flow";
        public const string MacroTrendsCashOnHandScraperPath = "/cash-on-hand";
        public const string StockAnalysisBalanceSpreadSheetPath = "/financials/balance-sheet/";
        public const string StockAnalysisStatisticsSheetPath = "/statistics";
        public const string StockAnalysisCashFlowScraperPath = "/financials/cash-flow-statement";
    }
}
