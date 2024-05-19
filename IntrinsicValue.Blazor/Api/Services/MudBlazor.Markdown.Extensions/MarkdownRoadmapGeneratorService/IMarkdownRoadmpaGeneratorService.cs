using MudBlazor.Markdown.Extensions.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudBlazor.Markdown.Extensions.MarkdownRoadmapGeneratorService
{
    public interface IMarkdownRoadmpaGeneratorService
    {
        List<TimelineEventDto> ParseRoadmapMarkdown(string roadmapMarkdown);
    }
}
