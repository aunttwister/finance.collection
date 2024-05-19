using MudBlazor.Markdown.Extensions.Domain.DTOs;

namespace Intrinsicly.Api.Services.UnpackMarkdown
{
    public interface IMarkdownButlerService
    {
        Dictionary<string, List<MarkdownInfoDto>> CategorizeMarkdownFiles(List<string> markdownFiles);
    }
}
