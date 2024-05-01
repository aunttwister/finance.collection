using FinanceScraper.Common.NodeResolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScraper.Common.Base
{
    public abstract class ScraperBaseCommand
    {
        public ScraperBaseCommand(string ticker)
        {
            Ticker = ticker;
        }
        public ScraperBaseCommand(string ticker, string path)
        {
            Ticker = ticker;
            Path = path;
        }
        public ScraperBaseCommand() { }
        public string BaseUrl { get; set; }
        public string Ticker { get; set; }
        public string Path { get; set; }
        public string FullUrl { get; private set; }

        public string GetUrl()
        {
            return FullUrl;
        }
        public abstract void ResolveUrl();
        public void SetFullUrl(string newUrl)
        {
            FullUrl = newUrl;
        }
    }
}
