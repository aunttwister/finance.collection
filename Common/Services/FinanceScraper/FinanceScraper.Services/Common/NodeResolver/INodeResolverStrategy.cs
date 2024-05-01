using FinanceScraper.Common.Base;
using FinanceScraper.Common.Init.Commands;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScraper.Common.NodeResolver
{
    public interface INodeResolverStrategy
    {
        Task<HtmlNode> ResolveNodeAsync(string url);
    }

}
