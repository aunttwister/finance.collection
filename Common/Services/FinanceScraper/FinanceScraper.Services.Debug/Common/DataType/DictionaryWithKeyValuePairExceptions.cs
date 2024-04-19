using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScraper.Common.DataType
{
    public class DictionaryWithKeyValuePairExceptions<T1, T2>
    {
        public Dictionary<T1, T2> Dictionary { get; set; }
        public KeyValuePair<Exception, Exception> KeyValuePairExceptions { get; set; }
        public DictionaryWithKeyValuePairExceptions(
            Dictionary<T1, T2> dictionary, 
            KeyValuePair<Exception, Exception> keyValuePairExceptions)
        {
            Dictionary = dictionary;
            KeyValuePairExceptions = keyValuePairExceptions;
        }
    }
}
