
namespace Intrinsicly.Api.Services.ReadWebContent
{
    public class ReadWebContentService : IReadWebContentService
    {
        public string GetMarkdownContent(IWebHostEnvironment _env, string directoryPath)
        {
            var filePath = Path.Combine(_env.WebRootPath, directoryPath);
            if (!File.Exists(filePath))
            {
                return "";
            }

            var content = File.ReadAllText(filePath);
            return content;
        }

        public List<string> GetMarkdownFiles(IWebHostEnvironment _env)
        {
            var markdownDir = Path.Combine(_env.WebRootPath, "markdown");
            return Directory.GetFiles(markdownDir, "*.md", SearchOption.AllDirectories)
                                 .Select(f => f.Replace(_env.WebRootPath + Path.DirectorySeparatorChar, "").Replace("\\", "/"))
                                 .ToList();
        }
    }
}
