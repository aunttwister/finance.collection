using System;
using System.Collections.Generic;
using System.Reflection;

namespace Financial.Collection.Link.Exceptions
{
    public class ExceptionExtractor
    {
        public static List<Exception> ExtractExceptions(object obj)
        {
            List<Exception> exceptions = new List<Exception>();
            if (obj == null)
                return exceptions;

            Type objType = obj.GetType();
            if (typeof(Exception).IsAssignableFrom(objType))
            {
                exceptions.Add((Exception)obj);
            }
            else if (obj is KeyValuePair<string, Exception>)
            {
                var kvp = (KeyValuePair<string, Exception>)obj;
                if (kvp.Value != null)
                    exceptions.Add(kvp.Value);
            }
            else if (obj.GetType() == typeof(string))
            { }
            else
            {
                foreach (PropertyInfo property in objType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    object value = property.GetValue(obj);
                    if (value != null)
                    {
                        exceptions.AddRange(ExtractExceptions(value));
                    }
                }
            }

            return exceptions;
        }
    }
}

