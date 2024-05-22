using MudBlazor.Markdown.Extensions.Domain.DTOs;

namespace Intrinsicly.WASM.Services.Markdown 
{ 
    public interface IMarkdownService
    {
        public MarkdownInfoDto CurrentMarkdown { get; set; }

        Task LoadCurrentMarkdownAsync();

        Task SaveCurrentMarkdownAsync(MarkdownInfoDto markdown);

        Task<string> GetMarkdownContentAsync(string urlPath);

        Task<KeyValuePair<MarkdownInfoDto, string>> GetMarkdownEntityAsync(string urlPath);

        Task<Dictionary<string, List<MarkdownInfoDto>>> GetCategorizedMarkdownFilesAsync();

        Task<List<TimelineEventDto>> GetParsedRoadmapAsync(string urlPath);
    }
}
