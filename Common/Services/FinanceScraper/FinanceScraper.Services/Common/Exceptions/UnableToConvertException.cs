using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScraper.Common.Exceptions
{
    public class UnableToConvertException : Exception
    {
        public UnableToConvertException(string message) : base(message){ }
    }
}
