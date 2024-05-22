using MudBlazor.Markdown.Extensions.Domain.DTOs;
using System.Net.Http.Json;

namespace Intrinsicly.WASM.Services.MarkdownContent
{
    public class MarkdownContentService : IMarkdownContentService
    {
        private readonly HttpClient _httpClient;

        public MarkdownContentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Dictionary<string, List<MarkdownInfoDto>>> GetCategorizedMarkdownFilesAsync()
        {
            return await _httpClient.GetFromJsonAsync<Dictionary<string, List<MarkdownInfoDto>>>("api/Markdown/files");
        }

        public async Task<KeyValuePair<MarkdownInfoDto, string>> GetMarkdownEntityAsync(string urlPath)
        {
            var url = $"api/Markdown/entity?urlPath={urlPath}";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<KeyValuePair<MarkdownInfoDto, string>>();
            }
            else
            {
                // Handle error response accordingly
                throw new HttpRequestException($"Request to {url} failed with status code {response.StatusCode}");
            }
        }

        public async Task<string> GetMarkdownContentAsync(string urlPath)
        {
            var url = $"api/Markdown/content?urlPath={urlPath}";
            return await _httpClient.GetStringAsync(url);
        }

        public async Task<List<TimelineEventDto>> GetParsedRoadmapAsync(string urlPath)
        {
            var url = $"api/Markdown/parseEntity?urlPath={urlPath}";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<TimelineEventDto>>();
            }
            else
            {
                // Handle error response accordingly
                throw new HttpRequestException($"Request to {url} failed with status code {response.StatusCode}");
            }
        }
    }

}
