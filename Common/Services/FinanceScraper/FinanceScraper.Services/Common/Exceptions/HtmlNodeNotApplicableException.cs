using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPortfolio.FinanceScraper.Common.Exceptions
{
    public class HtmlNodeNotApplicableException : Exception
    {
        public HtmlNodeNotApplicableException(string message) : base(message) { }
    }
}
