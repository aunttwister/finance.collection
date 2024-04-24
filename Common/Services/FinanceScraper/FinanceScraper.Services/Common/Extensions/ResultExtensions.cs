using Finance.Collection.Domain.FinanceScraper.Propagation;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScraper.Common.Extensions
{
    public static class ResultExtensions
    {
        public static MethodResult<T> ExecuteUntilFirstException<T>(this HtmlNode node, Func<MethodResult<T>>[] operations, [CallerMemberName] string methodName = "")
        {
            MethodResult<T> result = new MethodResult<T>();
            foreach (var operation in operations)
            {
                result = operation();
                if (!result.IsSuccessful)
                {
                    ApplicationException exception = new ApplicationException($"Error in {methodName}: {result.Exception.Message}");
                    // Return a new MethodResult with the minimal value and the exception
                    return new MethodResult<T>(exception);
                }
            }
            return result;
        }

        public static MethodResult<T> ExecuteUntilFirstException<T>(this HtmlNodeCollection nodeCollection, Func<MethodResult<T>>[] operations, [CallerMemberName] string methodName = "")
        {
            MethodResult<T> result = new MethodResult<T>();
            foreach (var operation in operations)
            {
                result = operation();
                if (!result.IsSuccessful)
                {
                    ApplicationException exception = new ApplicationException($"Error in {methodName}: {result.Exception.Message}");
                    // Return a new MethodResult with the minimal value and the exception
                    return new MethodResult<T>(exception);
                }
            }
            return result;
        }
    }

}

