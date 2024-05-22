using MudBlazor.Markdown.Extensions.Domain.DTOs;

namespace Intrinsicly.WASM.Services.MarkdownContent
{
    public interface IMarkdownContentService
    {
        Task<Dictionary<string, List<MarkdownInfoDto>>> GetCategorizedMarkdownFilesAsync();
        Task<string> GetMarkdownContentAsync(string urlPath);
        Task<KeyValuePair<MarkdownInfoDto, string>> GetMarkdownEntityAsync(string urlPath);
        Task<List<TimelineEventDto>> GetParsedRoadmapAsync(string directoryPath);
    }
}
