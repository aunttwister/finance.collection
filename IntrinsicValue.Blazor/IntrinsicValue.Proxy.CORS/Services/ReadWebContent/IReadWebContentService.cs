using MudBlazor.Markdown.Extensions.Domain.DTOs;

namespace Intrinsicly.Api.Services.ReadWebContent
{
    public interface IReadWebContentService
    {
        List<string> GetMarkdownFiles();
        string GetMarkdownContent(string urlPath);
        KeyValuePair<MarkdownInfoDto, string> GetMarkdownEntity(string urlPath);
    }
}
