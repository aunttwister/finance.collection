using FinanceScraper.Common.Propagation;
using HtmlAgilityPack;
using System.Runtime.CompilerServices;

namespace FinanceScraper.Common.Exceptions.ExceptionResolver
{
    public class ExceptionResolverService : IExceptionResolverService
    {
        public ApplicationException HandleException(Exception exception, string contextMessage) 
        {
            return new ApplicationException(contextMessage, exception);
        }
        public MethodResult<decimal> ConvertToDecimalExceptionResolver(string toConvert)
        {
            try
            {
                decimal data = Convert.ToDecimal(toConvert);
                return new MethodResult<decimal>(data);
            }
            catch (Exception ex)
            {
                ApplicationException exception = HandleException(ex, $"Failed to convert '{toConvert}' to decimal.");
                return new MethodResult<decimal>(0m, exception);
            }
        }

        public MethodResult<IEnumerable<decimal>> MultiConvertToDecimalExceptionResolver(IEnumerable<string> toConvert)
        {
            List<decimal> data = new List<decimal>();
            foreach (string value in toConvert)
            {
                try
                {
                    decimal converted = Convert.ToDecimal(toConvert);
                    data.Add(converted);
                }
                catch (Exception ex)
                {
                    ApplicationException exception = HandleException(ex, $"Failed to convert '{value}' to decimal.");
                    return new MethodResult<IEnumerable<decimal>>(data, exception);
                }
            }
            return new MethodResult<IEnumerable<decimal>>(data);
        }

        public MethodResult<T> HtmlNodeKeyCharacterNotFoundExceptionResolver<T>(HtmlNode toResolve, char keyCharacter)
        {
            try
            {
                if (toResolve is null)
                {
                    throw new ArgumentOutOfRangeException();
                }
                return new MethodResult<T>();
            }
            catch (Exception ex)
            {
                ApplicationException exception = HandleException(ex, $"Failed to resolve Html Node. Key character: '{keyCharacter}' for split operation is missing.");
                return new MethodResult<T>(exception);
            }
        }

        public MethodResult<T> HtmlNodeNotApplicableExceptionResolver<T>(HtmlNode toResolve)
        {
            try
            {
                if (toResolve.InnerHtml == "N/A")
                {
                    throw new ArgumentOutOfRangeException();
                }
                return new MethodResult<T>();
            }
            catch (Exception ex)
            {
                ApplicationException exception = HandleException(ex, "Failed to resolve Html Node. Data is not scrapable because value \"N/A\" has been found.");
                return new MethodResult<T>(exception);
            }
        }

        public MethodResult<T> HtmlNodeNullReferenceExceptionResolver<T>(HtmlNode toResolve)
        {
            try
            {
                if (toResolve is null)
                {
                    throw new ArgumentNullException();
                }
                return new MethodResult<T>();
            }
            catch (Exception ex)
            {
                ApplicationException exception = HandleException(ex, $"Failed to resolve Html Node. XPath doesn't exist.");
                return new MethodResult<T>(exception);
            }
        }

        public MethodResult<T> HtmlNodeCollectionNullReferenceExceptionResolver<T>(HtmlNodeCollection toResolve)
        {
            try
            {
                if (toResolve is null)
                {
                    throw new ArgumentNullException();
                }
                return new MethodResult<T>();
            }
            catch (Exception ex)
            {
                ApplicationException exception = HandleException(ex, $"Failed to resolve Html Node Collection. XPath doesn't exist.");
                return new MethodResult<T>(exception);
            }
        }
    }
}
