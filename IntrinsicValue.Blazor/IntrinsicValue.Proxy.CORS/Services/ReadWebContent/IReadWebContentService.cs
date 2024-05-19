namespace Intrinsicly.Api.Services.ReadWebContent
{
    public interface IReadWebContentService
    {
        string GetMarkdownContent(IWebHostEnvironment _env, string directoryPath);
        List<string> GetMarkdownFiles(IWebHostEnvironment _env);
    }
}
