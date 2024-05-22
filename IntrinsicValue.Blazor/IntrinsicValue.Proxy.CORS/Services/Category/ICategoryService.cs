using MudBlazor.Markdown.Extensions.Domain.DTOs;

namespace Intrinsicly.Api.Services.Category
{
    public interface ICategoryService
    {
        Dictionary<string, List<MarkdownInfoDto>> CategorizeFiles(List<string> filePaths);
    }
}
