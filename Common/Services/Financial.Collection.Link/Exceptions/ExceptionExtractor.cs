using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.Common.Results;
using Finance.Collection.Domain.FinanceScraper.Results;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Financial.Collection.Link.Exceptions
{
    public static class ExceptionExtractor
    {
        public static List<Exception> ExtractExceptions(object obj)
        {
            List<Exception> exceptions = new List<Exception>();

            if (obj == null)
            {
                return exceptions;
            }

            Type objType = obj.GetType();

            // Handle Exception directly
            if (obj is Exception exception)
            {
                exceptions.Add(exception);
            }
            // Handle KeyValuePair<string, Exception>
            else if (obj is KeyValuePair<string, Exception> kvpStringException)
            {
                if (kvpStringException.Value != null)
                {
                    exceptions.Add(kvpStringException.Value);
                }
            }
            // Handle KeyValuePair<Exception, Exception>
            else if (obj is KeyValuePair<Exception, Exception> kvpExceptionException)
            {
                if (kvpExceptionException.Key != null)
                {
                    exceptions.Add(kvpExceptionException.Key);
                }
                if (kvpExceptionException.Value != null)
                {
                    exceptions.Add(kvpExceptionException.Value);
                }
            }
            // Handle MethodResult
            else if (objType.IsGenericType && objType.GetGenericTypeDefinition() == typeof(MethodResult<>))
            {
                var exceptionProperty = objType.GetProperty("Exception");
                if (exceptionProperty != null)
                {
                    var exceptionResult = (Exception)exceptionProperty.GetValue(obj);
                    if (exceptionResult != null)
                    {
                        exceptions.Add(exceptionResult);
                    }
                }

                var dataProperty = objType.GetProperty("Data");
                if (dataProperty != null)
                {
                    var dataResult = dataProperty.GetValue(obj);
                    if (dataResult != null)
                    {
                        exceptions.AddRange(ExtractExceptions(dataResult));
                    }
                }
            }
            // Handle MethodResultDictionary
            else if (objType.IsGenericType && objType.GetGenericTypeDefinition() == typeof(MethodResultDictionary<,>))
            {
                var keyValuePairExceptionsProperty = objType.GetProperty("KeyValuePairExceptions");
                if (keyValuePairExceptionsProperty != null)
                {
                    var keyValuePairExceptions = (KeyValuePair<Exception, Exception>)keyValuePairExceptionsProperty.GetValue(obj);
                    if (keyValuePairExceptions.Key != null)
                    {
                        exceptions.Add(keyValuePairExceptions.Key);
                    }
                    if (keyValuePairExceptions.Value != null)
                    {
                        exceptions.Add(keyValuePairExceptions.Value);
                    }
                }

                var dataProperty = objType.GetProperty("Data");
                if (dataProperty != null)
                {
                    var dataResult = dataProperty.GetValue(obj);
                    if (dataResult != null)
                    {
                        exceptions.AddRange(ExtractExceptions(dataResult));
                    }
                }
            }
            // Handle Dictionary
            else if (obj is IDictionary dictionary)
            {
                foreach (object value in dictionary.Values)
                {
                    exceptions.AddRange(ExtractExceptions(value));
                }
            }
            // Handle IEnumerable
            else if (obj is IEnumerable enumerable && objType != typeof(string))
            {
                foreach (object value in enumerable)
                {
                    exceptions.AddRange(ExtractExceptions(value));
                }
            }
            else
            {
                // Handle other objects with properties and ICombinedResult
                exceptions.AddRange(HandleObjectProperties(obj, objType));
            }

            return exceptions;
        }

        private static List<Exception> HandleObjectProperties(object obj, Type objType)
        {
            List<Exception> exceptions = new List<Exception>();

            // Handle properties
            foreach (PropertyInfo property in objType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!property.CanRead || property.GetIndexParameters().Length > 0)
                {
                    continue;
                }

                object value = property.GetValue(obj);
                if (value != null)
                {
                    exceptions.AddRange(ExtractExceptions(value));
                }
            }

            // Handle ICombinedResult
            if (obj is ICombinedResult)
            {
                MethodInfo getAllResultsMethod = objType.GetMethod("GetAllResults");
                if (getAllResultsMethod != null)
                {
                    var results = (IDictionary)getAllResultsMethod.Invoke(obj, null);
                    foreach (object value in results.Values)
                    {
                        exceptions.AddRange(ExtractExceptions(value));
                    }
                }
            }

            return exceptions;
        }
    }

}

