using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPortfolio.FinanceScraper.Common.Exceptions.ExceptionResolver
{
    public interface IExceptionResolverService
    {
        public decimal ConvertToDecimalExceptionResolver(string toConvert, string commonExceptionSuffix);
        public void HtmlNodeNullReferenceExceptionResolver(HtmlNode toResolve, string commonExceptionSuffix);
        public void HtmlNodeNotApplicableExceptionResolver(HtmlNode toResolve, string commonExceptionSuffix);
        public void HtmlNodeKeyCharacterNotFoundExceptionResolver(HtmlNode toResolve, char keyCharacter, string commonExceptionSuffix);
        public void HtmlNodeCollectionNullReferenceExceptionResolver(HtmlNodeCollection toResolve, string commonExceptionSuffix);
    }
}
