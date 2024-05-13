using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Collection.Domain.FinanceScraper.Exceptions
{
    public class UnresolvableHtmlNodeException : ApplicationException
    {
        public UnresolvableHtmlNodeException(string message, Exception exception) : base(message, exception) { }
    }
}
