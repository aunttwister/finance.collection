namespace Intrinsicly.Api.Services.FileWorm
{
    public interface IFileWormService
    {
        string ReadFileContent(string filePath);
        List<string> GetFiles(string directoryPath, string searchPattern);
    }
}
