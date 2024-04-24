using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Finance.Collection.Domain.FinanceScraper.DataSets.Base;

namespace FinanceScraper.Common.Base
{
    public interface IScrapeServiceStrategy<T1, T2> where T1 : ScraperBaseCommand
                                                    where T2 : IFinanceDataSet
    {
        public Task<T2> ExecuteScrape(T1 request);
    }
}
