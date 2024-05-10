using Finance.Collection.Domain.FinanceScraper.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Collection.Domain.Common.Helpers.AutoMapper
{
    public static class MappingHelpers
    {
        public static decimal MapCurrentPrice(CombinedScrapeResult src)
        {
            var result = src.GetResult<CurrentPriceScrapeResult>();
            return result?.CurrentPrice.CurrentPrice.IsSuccessful == true ? result.CurrentPrice.CurrentPrice.Data : 0m;
        }

        public static decimal MapEPS(CombinedScrapeResult src)
        {
            var result = src.GetResult<GrahamScrapeResult>();
            return result?.Summary.Eps.IsSuccessful == true ? result.Summary.Eps.Data : 0m;
        }

        public static decimal MapExpectedFiveYearGrowth(CombinedScrapeResult src)
        {
            var result = src.GetResult<GrahamScrapeResult>();
            return result?.Analysis.FiveYearGrowth.IsSuccessful == true ? result.Analysis.FiveYearGrowth.Data : 0m;
        }

        public static decimal MapSharesOutstanding(CombinedScrapeResult src)
        {
            var result = src.GetResult<DCFScrapeResult>();
            return result?.Statistics.SharesOutstanding.IsSuccessful == true ? result.Statistics.SharesOutstanding.Data : 0m;
        }

        public static decimal MapTTMCashAndCashEquivalents(CombinedScrapeResult src)
        {
            var result = src.GetResult<DCFScrapeResult>();
            return result?.BalanceSheet.TTMCashEquivalents.IsSuccessful == true ? result.BalanceSheet.TTMCashEquivalents.Data : 0m;
        }

        public static decimal MapTTMTotalDebt(CombinedScrapeResult src)
        {
            var result = src.GetResult<DCFScrapeResult>();
            return result?.BalanceSheet.TTMTotalDebt.IsSuccessful == true ? result.BalanceSheet.TTMTotalDebt.Data : 0m;
        }
    }

}
