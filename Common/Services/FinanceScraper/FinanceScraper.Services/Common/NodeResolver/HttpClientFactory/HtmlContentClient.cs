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
            string proxyUrl = $"https://localhost:5001/api/proxy?url={Uri.EscapeDataString(url)}";
            

            // Fetch the HTML content
            var response = await _httpClient.GetAsync(proxyUrl);
            //response.EnsureSuccessStatusCode();  // Ensure successful response
            return await response.Content.ReadAsStringAsync();
        }
    }
}