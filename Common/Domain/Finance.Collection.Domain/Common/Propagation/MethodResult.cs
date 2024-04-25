using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Collection.Domain.Common.Propagation
{
    public class MethodResult<T>
    {
        public T Data { get; private set; }
        public Exception Exception { get; private set; }
        public bool IsSuccessful => Exception == null;

        public MethodResult()
        {

        }
        public MethodResult(T data)
        {
            Data = data;
        }
        public MethodResult(Exception exception) : this(GetMinimalValue<T>(), exception) { }
        public MethodResult(T data, Exception exception)
        {
            Data = data;
            Exception = exception;
        }

        private static T GetMinimalValue<T>()
        {
            if (typeof(T) == typeof(decimal))
            {
                return (T)(object)0m;
            }
            if (typeof(T) == typeof(string))
            {
                return (T)(object)"";
            }
            return default;
        }

        public void AssignData(T data)
        {
            Data = data;
        }
    }
}
