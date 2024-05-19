using Intrinsicly.Api.Services.ReadWebContent;
using Intrinsicly.Api.Services.UnpackMarkdown;
using MudBlazor.Markdown.Extensions.ServiceRegistar;

namespace Intrinsicly.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<IReadWebContentService, ReadWebContentService>();
            builder.Services.AddMarkdownButlerServices();

            // Add CORS policy to allow requests from your Blazor WebAssembly app
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowBlazorApp",
                    builder => builder.AllowAnyOrigin()
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });

            builder.Services.AddHttpClient(); // Add HttpClientFactory
            builder.Services.AddControllers(); // Add controllers

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors("AllowBlazorApp"); // Enable CORS policy
            app.UseEndpoints(endpoints => endpoints.MapControllers());

            app.Run();
        }
    }
}