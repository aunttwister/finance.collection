using HtmlAgilityPack;
using FinanceScraper.Common.Base;
using FinanceScraper.Common.DataSets;
using FinanceScraper.Common.Extensions;
using FinanceScraper.MacroTrends.CashFlow.Commands;

namespace FinanceScraper.MacroTrends.CashFlow
{
    public class MacroTrendsCashOnHandScrapeService : IScrapeServiceStrategy<MacroTrendsCashOnHandScraperCommand, CashFlowDataSet>
    {
        public MacroTrendsCashOnHandScrapeService() { }
        public async Task<CashFlowDataSet> ExecuteScrape(MacroTrendsCashOnHandScraperCommand request)
        {
            string fullUrl = await ResolveHistoricalCashFlowUrl(request.FullUrl, request.ActionPath);

            request.SetFullUrl(fullUrl);

            HtmlNode node = await request.NodeResolverAsync();

            Dictionary<string, decimal> historicalYearCashFlows = GetHistoricalYearCashFlow(node);

            return new CashFlowDataSet() { HistoricalYearCashFlows = historicalYearCashFlows };
        }

        private static async Task<string> ResolveHistoricalCashFlowUrl(string url, string path)
        {
            Uri uri = new Uri(url);

            using var client = new HttpClient(new HttpClientHandler
            {
                AllowAutoRedirect = false,
            }, true);


            client.DefaultRequestHeaders.Add("authority", "www.macrotrends.net");
            client.DefaultRequestHeaders.Add("method", "GET");
            client.DefaultRequestHeaders.Add("scheme", "https");
            client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            client.DefaultRequestHeaders.Add("Accept-Language", "en-GB,en;q=0.9,sr-RS;q=0.8,sr;q=0.7,en-US;q=0.6");
            client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            client.DefaultRequestHeaders.Add("Pragma", "no-cache");
            client.DefaultRequestHeaders.Add("Sec-Ch-Ua", "\"Not_A Brand\";v=\"8\", \"Chromium\";v=\"120\", \"Google Chrome\";v=\"120\"");
            client.DefaultRequestHeaders.Add("Sec-Ch-Ua-Mobile", "?0");
            client.DefaultRequestHeaders.Add("Sec-Ch-Ua-Platform", "Windows");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "document");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "navigate");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Site", "cross-site");
            client.DefaultRequestHeaders.Add("Sec-Fetch-User", "?1");
            client.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");

            using var response = await client.GetAsync(uri);
            uri = new Uri(response.Headers.GetValues("Location").First());

            return url = uri.ToString() + path;
        }

        private static Dictionary<string, decimal> GetHistoricalYearCashFlow(HtmlNode node)
        {
            IEnumerable<HtmlNode> yearNodesTableRows = node.SelectNodes("//table[@class='historical_data_table table']/tbody").Nodes();

            if (yearNodesTableRows is null)
                throw new Exception();

            IEnumerable<HtmlNode> yearNodes = yearNodesTableRows.Select(node => node.SelectSingleNode("//td[0]"));

            HtmlNodeCollection cashFlowNodes = node.SelectNodes("//table[contains(@class, 'historical_data_table)']/tbody/tr/td[1]");

            IEnumerable<string> years = yearNodes.Select(year => year.InnerHtml);

            IEnumerable<decimal> cashFlows = cashFlowNodes.Select(cashFlow => Convert.ToDecimal(cashFlow.InnerHtml));

            Dictionary<string, decimal> historicalYearCashFlows = years.Zip(cashFlows, (key, value) => new { key, value }).ToDictionary(keyValuePair => keyValuePair.key, keyValuePair => keyValuePair.value);

            return historicalYearCashFlows;

        }
    }
}