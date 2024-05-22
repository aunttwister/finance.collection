using Intrinsicly.Api.Services.MarkdownButler;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Markdown.Extensions.MarkdownRoadmapGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudBlazor.Markdown.Extensions.ServiceRegistar
{
    public static class RegisterMarkdownButler
    {
        public static void AddMarkdownButlerServices(this IServiceCollection services)
        {
            services.AddScoped<IMarkdownButlerService, MarkdownButlerService>();
            services.AddScoped<IMarkdownRoadmpaGeneratorService, MarkdownRoadmpaGeneratorService>();
        }
    }
}
