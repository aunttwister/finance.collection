using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Collection.Domain.Common.Propagation
{
    public class MethodResultDictionary<T1, T2>
    {
        public Dictionary<T1, T2> Data { get; set; }
        public KeyValuePair<Exception, Exception> KeyValuePairExceptions { get; set; }
        public MethodResultDictionary(
            Dictionary<T1, T2> data,
            KeyValuePair<Exception, Exception> keyValuePairExceptions)
        {
            Data = data;
            KeyValuePairExceptions = keyValuePairExceptions;
        }
    }
}
