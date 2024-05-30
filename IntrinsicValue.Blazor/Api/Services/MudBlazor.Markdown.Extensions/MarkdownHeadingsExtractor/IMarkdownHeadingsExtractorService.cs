using Markdig.Syntax;
using Markdig;
using MudBlazor.Markdown.Extensions.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudBlazor.Markdown.Extensions.MarkdownHeadingsExtractor
{
    public interface IMarkdownHeadingsExtractorService
    {
        List<HeadingDto> ExtractHeadings(string markdownContent);
    }
}
