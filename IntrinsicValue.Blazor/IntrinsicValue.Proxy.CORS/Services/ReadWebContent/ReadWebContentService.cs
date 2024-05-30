using Intrinsicly.Api.Services.Category;
using Intrinsicly.Api.Services.FileWorm;
using MudBlazor.Markdown.Extensions.Domain.DTOs;
using MudBlazor.Markdown.Extensions.MarkdownHeadingsExtractor;

namespace Intrinsicly.Api.Services.ReadWebContent
{
    public class ReadWebContentService : IReadWebContentService
    {
        private readonly IFileWormService _fileService;
        private readonly ICategoryService _categoryService;
        private readonly IMarkdownHeadingsExtractorService _markdownHeadingsExtractorService;

        public ReadWebContentService(
            IFileWormService fileService, 
            ICategoryService categoryService, 
            IMarkdownHeadingsExtractorService markdownHeadingsExtractorService)
        {
            _fileService = fileService;
            _categoryService = categoryService;
            _markdownHeadingsExtractorService = markdownHeadingsExtractorService;
        }

        public List<string> GetMarkdownFiles()
        {
            return _fileService.GetFiles("markdown", "*.md");
        }

        public KeyValuePair<MarkdownInfoDto, string> GetMarkdownEntity(string urlPath)
        {
            List<string> markdownFiles = GetMarkdownFiles();
            Dictionary<string, List<MarkdownInfoDto>> categorizedFiles = _categoryService.CategorizeFiles(markdownFiles);

            MarkdownInfoDto markdownInfoDto = categorizedFiles
                .SelectMany(ctg => ctg.Value)
                .First(md => md.UrlPath == urlPath);

            if (markdownInfoDto == null)
            {
                return new KeyValuePair<MarkdownInfoDto, string>(null, "");
            }

            string content = GetMarkdownContentFromDirectory(markdownInfoDto.DirectoryPath);

            markdownInfoDto.Headings = _markdownHeadingsExtractorService.ExtractHeadings(content);

            return new KeyValuePair<MarkdownInfoDto, string>(markdownInfoDto, content);
        }

        public string GetMarkdownContent(string urlPath)
        {
            List<string> markdownFiles = GetMarkdownFiles();
            Dictionary<string, List<MarkdownInfoDto>> categorizedFiles = _categoryService.CategorizeFiles(markdownFiles);

            MarkdownInfoDto markdownInfoDto = categorizedFiles
                .SelectMany(ctg => ctg.Value)
                .First(md => md.UrlPath == urlPath);

            return GetMarkdownContentFromDirectory(markdownInfoDto.DirectoryPath);
        }

        private string GetMarkdownContentFromDirectory(string directoryPath)
        {
            return _fileService.ReadFileContent(directoryPath);
        }
    }
}
