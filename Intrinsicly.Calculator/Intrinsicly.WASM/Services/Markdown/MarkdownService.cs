using Intrinsicly.WASM.Services.LocalStorage;
using MudBlazor.Markdown.Extensions.Domain.DTOs;
using System.Net.Http.Json;

namespace Intrinsicly.WASM.Services.Markdown
{
    public class MarkdownService
    {
        private readonly HttpClient _httpClient;
        private readonly LocalStorageService _localStorageService;
        private const string CurrentMarkdownKey = "currentMarkdown";

        public MarkdownInfoDto CurrentMarkdown { get; set; }

        public MarkdownService(HttpClient httpClient, LocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        public async Task<Dictionary<string, List<MarkdownInfoDto>>> GetCategorizedMarkdownFilesAsync()
        {
            return await _httpClient.GetFromJsonAsync<Dictionary<string, List<MarkdownInfoDto>>>("api/Markdown/files");
        }

        public async Task<string> GetMarkdownContentAsync(string directoryPath)
        {
            var url = $"api/Markdown/content?directoryPath={directoryPath}";
            return await _httpClient.GetStringAsync(url);
        }
        public async Task<List<TimelineEventDto>> GetParsedRoadmapAsync(string directoryPath)
        {
            var url = $"api/Markdown/roadmap?directoryPath={directoryPath}";
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

        public async Task SaveCurrentMarkdownAsync(MarkdownInfoDto markdown)
        {
            CurrentMarkdown = markdown;
            await _localStorageService.SetItemAsync(CurrentMarkdownKey, markdown);
        }

        public async Task LoadCurrentMarkdownAsync()
        {
            CurrentMarkdown = await _localStorageService.GetItemAsync<MarkdownInfoDto>(CurrentMarkdownKey);
        }
    }
}