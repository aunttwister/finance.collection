using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace FinanceScraper.Common.NodeResolver.HttpClientFactory
{
    public class HtmlContentClient
    {
        private readonly HttpClient _httpClient;

        public HtmlContentClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetHtmlContentAsync(string url)
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml", 0.9));
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*", 0.8));

            var response = await _httpClient.GetAsync(url);
            //response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}