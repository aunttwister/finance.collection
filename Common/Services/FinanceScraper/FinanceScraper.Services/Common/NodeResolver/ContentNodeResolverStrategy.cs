using FinanceScraper.Common.Base;
using FinanceScraper.Common.Init.Commands;
using FinanceScraper.Common.NodeResolver.HttpClientFactory;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScraper.Common.NodeResolver
{
    public class ContentNodeResolverStrategy : INodeResolverStrategy
    {
        private readonly HtmlContentClient _htmlContentClient;

        public ContentNodeResolverStrategy(HtmlContentClient htmlContentClient)
        {
            _htmlContentClient = htmlContentClient;
        }

        public async Task<HtmlNode> ResolveNodeAsync(string url)
        {
            string htmlContent = await _htmlContentClient.GetHtmlContentAsync(url);

            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(htmlContent)))
            {
                var doc = new HtmlDocument();
                doc.Load(stream);
                return doc.DocumentNode;
            }
        }
    }
}
