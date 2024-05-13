using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Collection.Domain.FinanceScraper.Exceptions
{
    public class TickerValidationException : ApplicationException
    {
        public TickerValidationException(string message) : base(message) { }
    }
}
