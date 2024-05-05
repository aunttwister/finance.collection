using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.FinanceScraper.DataSets;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScraper.YahooFinance.CurrentPriceScraper.Commands
{
    public class YahooFinanceCurrentPriceScraperCommand : YahooFinanceScraperBaseCommand, IRequest<CurrentPriceDataSet>
    {
        public YahooFinanceCurrentPriceScraperCommand(string ticker, string path) : base(ticker, path) { }
    }
}
