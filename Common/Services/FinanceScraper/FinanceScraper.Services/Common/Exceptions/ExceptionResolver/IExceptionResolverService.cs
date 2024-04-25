using Finance.Collection.Domain.Common.Propagation;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScraper.Common.Exceptions.ExceptionResolver
{
    public interface IExceptionResolverService
    {
        public MethodResult<decimal> ConvertToDecimalExceptionResolver(string toConvert);
        public MethodResult<T> HtmlNodeNullReferenceExceptionResolver<T>(HtmlNode toResolve);
        public MethodResult<T> HtmlNodeNotApplicableExceptionResolver<T>(HtmlNode toResolve);
        public MethodResult<T> HtmlNodeKeyCharacterNotFoundExceptionResolver<T>(HtmlNode toResolve, char keyCharacter);
        public MethodResult<T> HtmlNodeCollectionNullReferenceExceptionResolver<T>(HtmlNodeCollection toResolve);
        public MethodResult<IEnumerable<decimal>> MultiConvertToDecimalExceptionResolver(IEnumerable<string> toConvert);
    }
}
