using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPortfolio.FinanceScraper.Common.Exceptions
{
    public class HtmlNodeKeyCharacterNotFoundException : Exception
    {
        public HtmlNodeKeyCharacterNotFoundException(string message) : base(message){ }
    }
}
