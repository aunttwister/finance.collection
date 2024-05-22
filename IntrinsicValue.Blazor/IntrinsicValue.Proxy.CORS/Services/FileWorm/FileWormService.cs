namespace Intrinsicly.Api.Services.FileWorm
{
    public class FileWormService : IFileWormService
    {
        private readonly IWebHostEnvironment _env;

        public FileWormService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string ReadFileContent(string filePath)
        {
            var fullPath = Path.Combine(_env.WebRootPath, filePath);
            if (!File.Exists(fullPath))
            {
                return "";
            }
            return File.ReadAllText(fullPath);
        }

        public List<string> GetFiles(string directoryPath, string searchPattern)
        {
            var fullPath = Path.Combine(_env.WebRootPath, directoryPath);
            return Directory.GetFiles(fullPath, searchPattern, SearchOption.AllDirectories)
                            .Select(f => f.Replace(_env.WebRootPath + Path.DirectorySeparatorChar, "").Replace("\\", "/"))
                            .ToList();
        }
    }

}
