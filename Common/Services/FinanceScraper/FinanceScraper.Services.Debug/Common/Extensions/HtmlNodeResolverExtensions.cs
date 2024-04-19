using HtmlAgilityPack;
using FinanceScraper.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScraper.Common.Extensions
{
    public static class HtmlNodeResolverExtensions
    {
        public static HtmlNode NodeResolver(this ScraperBaseCommand request)
        {
            string url = request.FullUrl;
            var web = new HtmlWeb();
            var doc = web.Load(url);

            return doc.DocumentNode;
        }

        public static async Task<HtmlNode> NodeResolverAsync(this ScraperBaseCommand request)
        {
            string url = request.FullUrl;
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(url);

            return doc.DocumentNode;
        }
        public static async Task<HtmlNode> DelayNodeResolverAsync(this ScraperBaseCommand request)
        {
            string url = request.FullUrl;
            var web = new HtmlWeb();
            var task = web.LoadFromWebAsync(url);
            HtmlDocument doc = new();
            if (task.Wait(2000, CancellationToken.None))
                doc = await task;

            return doc.DocumentNode;
        }
    }
}
