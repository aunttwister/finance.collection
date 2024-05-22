using Intrinsicly.Api.Services.Category;
using Intrinsicly.Api.Services.FileWorm;
using Intrinsicly.Api.Services.ReadWebContent;

namespace Intrinsicly.Api.Services.ServiceRegistar
{
    public static class RegisterMarkdownServices
    {
        public static void AddMarkdownServices(this IServiceCollection services)
        {
            services.AddScoped<IReadWebContentService, ReadWebContentService>();
            services.AddScoped<IFileWormService, FileWormService>();
            services.AddScoped<ICategoryService, CategoryService>();
        }
    }
}
