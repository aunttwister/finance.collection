using Intrinsicly.Api.Services.MarkdownButler;
using MudBlazor.Markdown.Extensions.Domain.DTOs;

namespace Intrinsicly.Api.Services.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly IMarkdownButlerService _markdownButlerService;

        public CategoryService(IMarkdownButlerService markdownButlerService)
        {
            _markdownButlerService = markdownButlerService;
        }

        public Dictionary<string, List<MarkdownInfoDto>> CategorizeFiles(List<string> filePaths)
        {
            return _markdownButlerService.CategorizeMarkdownFiles(filePaths);
        }
    }
}
