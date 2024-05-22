using Intrinsicly.WASM.Services.LocalStorage;
using Intrinsicly.WASM.Services.MarkdownContent;
using MudBlazor.Markdown.Extensions.Domain.DTOs;
using System.Net.Http.Json;

namespace Intrinsicly.WASM.Services.Markdown
{
    public class MarkdownService : IMarkdownService
    {
        private readonly IMarkdownContentService _markdownContentService;
        private readonly ILocalStorageService _localStorageService;
        private const string CurrentMarkdownKey = "currentMarkdown";

        public MarkdownInfoDto CurrentMarkdown { get; set; }

        public MarkdownService(IMarkdownContentService markdownContentService, ILocalStorageService localStorageService)
        {
            _markdownContentService = markdownContentService;
            _localStorageService = localStorageService;
        }

        public async Task LoadCurrentMarkdownAsync()
        {
            CurrentMarkdown = await _localStorageService.GetItemAsync<MarkdownInfoDto>(CurrentMarkdownKey);
        }

        public async Task SaveCurrentMarkdownAsync(MarkdownInfoDto markdown)
        {
            CurrentMarkdown = markdown;
            await _localStorageService.SetItemAsync(CurrentMarkdownKey, markdown);
        }

        public async Task<string> GetMarkdownContentAsync(string urlPath)
        {
            return await _markdownContentService.GetMarkdownContentAsync(urlPath);
        }

        public async Task<KeyValuePair<MarkdownInfoDto, string>> GetMarkdownEntityAsync(string urlPath)
        {
            return await _markdownContentService.GetMarkdownEntityAsync(urlPath);
        }

        public async Task<Dictionary<string, List<MarkdownInfoDto>>> GetCategorizedMarkdownFilesAsync()
        {
            return await _markdownContentService.GetCategorizedMarkdownFilesAsync();
        }

        public async Task<List<TimelineEventDto>> GetParsedRoadmapAsync(string urlPath)
        {
            return await _markdownContentService.GetParsedRoadmapAsync(urlPath);
        }
    }

}