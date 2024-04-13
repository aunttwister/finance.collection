using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FinanceScraper.Common.Exceptions.ExceptionResolver
{
    public class ExceptionResolverService : IExceptionResolverService
    {
        public decimal ConvertToDecimalExceptionResolver(string toConvert, string commonExceptionSuffix)
        {
            decimal convertedValue;
            try
            {
                convertedValue = Convert.ToDecimal(toConvert);
            }
            catch (FormatException)
            {
                string message = string.Format("Unable to convert the following value: {0}", toConvert) + commonExceptionSuffix;
                throw new UnableToConvertException(message);
            }

            return convertedValue;
        }

        public void HtmlNodeKeyCharacterNotFoundExceptionResolver(HtmlNode toResolve, char keyCharacter, string commonExceptionSuffix)
        {
            string message;

            if (toResolve is null)
            {
                message = string.Format("Unable to resolve Html Node") + commonExceptionSuffix;
                throw new HtmlNodeNullReferenceException(message);
            }
        }

        public void HtmlNodeNotApplicableExceptionResolver(HtmlNode toResolve, string commonExceptionSuffix)
        {
            string message;

            if (toResolve.InnerHtml == "N/A")
            {
                message = string.Format("Data is not scrapable because value \"N/A\" has been found") + commonExceptionSuffix;
                throw new HtmlNodeNotApplicableException(message);
            }
        }

        public void HtmlNodeNullReferenceExceptionResolver(HtmlNode toResolve, string commonExceptionSuffix)
        {
            string message;

            if (toResolve is null)
            {
                message = string.Format("Unable to resolve Html Node") + commonExceptionSuffix;
                throw new HtmlNodeNullReferenceException(message);
            }
        }

        public void HtmlNodeCollectionNullReferenceExceptionResolver(HtmlNodeCollection toResolve, string commonExceptionSuffix)
        {
            string message;

            if (toResolve is null)
            {
                message = string.Format("Unable to resolve Html Node Collection") + commonExceptionSuffix;
                throw new HtmlNodeNullReferenceException(message);
            }
        }
    }
}
