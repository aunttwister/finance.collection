using FinanceScraper.Common.Base;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScraper.Common.NodeResolver
{
    public class WebNodeResolverStrategy : INodeResolverStrategy
    {
        public async Task<HtmlNode> ResolveNodeAsync(string url)
        {
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(url);
            return doc.DocumentNode;
        }
    }
}
