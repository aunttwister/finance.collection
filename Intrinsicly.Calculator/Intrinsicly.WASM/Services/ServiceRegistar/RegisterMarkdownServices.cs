using Intrinsicly.WASM.Services.LocalStorage;
using Intrinsicly.WASM.Services.Markdown;
using Intrinsicly.WASM.Services.MarkdownContent;

namespace Intrinsicly.WASM.Services.ServiceRegistar
{
    public static class RegisterMarkdownServices
    {
        public static void AddMarkdownServices(this IServiceCollection services)
        {
            services.AddScoped<IMarkdownService, MarkdownService>();
            services.AddScoped<ILocalStorageService, LocalStorageService>();
            services.AddScoped<IMarkdownContentService, MarkdownContentService>();
        }
    }
}
