using MudBlazor.Markdown.Extensions.Domain.DTOs;

namespace Intrinsicly.Api.Services.MarkdownButler
{
    public class MarkdownButlerService : IMarkdownButlerService
    {
        public Dictionary<string, List<MarkdownInfoDto>> CategorizeMarkdownFiles(List<string> markdownFiles)
        {
            var categorizedFiles = markdownFiles.GroupBy(f => Path.GetDirectoryName(f)?.Split('\\').Last())
                                   .Select(g => new
                                   {
                                       Category = g.Key,
                                       Priority = GetPriority(g.Key),
                                       Files = g.Select(f => new MarkdownInfoDto
                                       {
                                           OriginalName = Path.GetFileNameWithoutExtension(f),
                                           DisplayName = FormatDisplayName(GetOriginalName(Path.GetFileNameWithoutExtension(f))),
                                           Priority = GetPriority(Path.GetFileNameWithoutExtension(f)),
                                           UrlPath = GetUrlPath(f),
                                           DirectoryPath = f
                                       })
                                       .OrderBy(f => f.Priority)
                                       .ToList()
                                   })
                                   .OrderBy(g => g.Priority)
                                   .ToDictionary(g => FormatDisplayName(GetOriginalName(g.Category)), g => g.Files);

            return categorizedFiles;
        }

        private int GetPriority(string fileName)
        {
            var parts = fileName.Split('_');
            if (parts.Length > 1 && int.TryParse(parts.Last(), out int priority))
            {
                return priority;
            }
            return int.MaxValue; // If no priority is found, place it at the end with the least priority.
        }

        private string GetOriginalName(string fileName)
        {
            var parts = fileName.Split('_');
            if (parts.Length > 1 && int.TryParse(parts.Last(), out _))
            {
                return string.Join('_', parts.Take(parts.Length - 1));
            }
            return fileName;
        }

        private string GetUrlPath(string filePath)
        {
            var fullDirectoryName = Path.GetDirectoryName(filePath)?.Replace("\\", "/").Split('/').Last();
            string directoryName = GetOriginalName(fullDirectoryName);
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var displayName = GetOriginalName(fileName);
            return $"{directoryName}/{displayName}";
        }

        private string FormatDisplayName(string fileName)
        {
            // Replace hyphens with spaces
            var formattedName = fileName.Replace("-", " ");
            // Capitalize each word
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(formattedName);
        }
    }
}

