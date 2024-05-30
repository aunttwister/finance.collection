using Markdig.Syntax;
using Markdig;
using MudBlazor.Markdown.Extensions.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace MudBlazor.Markdown.Extensions.MarkdownHeadingsExtractor
{
    public class MarkdownHeadingsExtractorService : IMarkdownHeadingsExtractorService
    {
        public List<HeadingDto> ExtractHeadings(string markdownContent)
        {
            // Parse the markdown content
            var pipeline = new MarkdownPipelineBuilder().Build();
            var document = Markdig.Markdown.Parse(markdownContent, pipeline);

            // Initialize the list of headings
            var rootHeadings = new List<HeadingDto>();
            var stack = new Stack<HeadingDto>();

            // Traverse the document to extract headers and add IDs
            foreach (var node in document)
            {
                if (node is HeadingBlock headingBlock)
                {
                    var headingText = string.Join("", headingBlock.Inline.Select(x => x.ToString()));
                    var headingId = GenerateId(headingText);

                    var headingDto = new HeadingDto
                    {
                        Heading = headingText,
                        Indent = headingBlock.Level - 1,
                        NestedHeadings = new List<HeadingDto>()
                    };

                    // Add the ID to the heading block
                    headingBlock.SetData("id", headingId);

                    while (stack.Count > 0 && stack.Peek().Indent >= headingDto.Indent)
                    {
                        stack.Pop();
                    }

                    if (stack.Count > 0)
                    {
                        stack.Peek().NestedHeadings.Add(headingDto);
                    }
                    else
                    {
                        rootHeadings.Add(headingDto);
                    }

                    stack.Push(headingDto);
                }
            }

            return rootHeadings;
        }

        private string GenerateId(string headingText)
        {
            // Generate a URL-friendly ID from the heading text
            return Regex.Replace(headingText.ToLower(), @"\s+", "-");
        }
    }
}
