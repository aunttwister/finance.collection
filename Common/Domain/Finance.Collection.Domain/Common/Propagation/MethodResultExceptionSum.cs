using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Collection.Domain.Common.Propagation
{
    public class MethodResultExceptionSum<T>
    {
        public MethodResultExceptionSum()
        {
            Exceptions = new List<Exception>();
        }
        public MethodResultExceptionSum(T data)
        {
            Data = data;
            Exceptions = new List<Exception>();
        }
        public MethodResultExceptionSum(T data, List<Exception> exceptions)
        {
            Data = data;
            Exceptions = exceptions;
        }
        public T Data { get; private set; }
        public List<Exception> Exceptions { get; private set; }
        public bool IsSuccessful => Exceptions.Count == 0;

        public void AssignData(T data)
        {
            Data = data;
        }

        public void AddException(Exception exception)
        {
            Exceptions.Add(exception);
        }

        public void AddExceptionRange(List<Exception> newException)
        {
            Exceptions.AddRange(newException);
        }
    }
}
