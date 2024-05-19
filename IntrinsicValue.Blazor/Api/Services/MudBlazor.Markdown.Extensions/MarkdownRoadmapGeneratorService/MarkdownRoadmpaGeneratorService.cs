using Markdig;
using Markdig.Syntax;
using Markdig.Helpers;
using MudBlazor.Markdown.Extensions.Domain.DTOs;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Linq;
using Markdig.Parsers;
using System.Threading.Tasks;

namespace MudBlazor.Markdown.Extensions.MarkdownRoadmapGeneratorService
{
    public class MarkdownRoadmpaGeneratorService : IMarkdownRoadmpaGeneratorService
    {
        private string sourceMarkdown;
        public List<TimelineEventDto> ParseRoadmapMarkdown(string roadmapMarkdown)
        {
            sourceMarkdown = RemoveBreaklines(roadmapMarkdown);
            MarkdownDocument document = ParseDocument(sourceMarkdown);

            return ExtractTimelineEvents(document);
        }

        private string RemoveBreaklines(string markdown)
        {
            return markdown.Replace("<br />", "").Replace("<br/>", "");
        } 

        private MarkdownDocument ParseDocument(string markdown)
        {
            var pipeline = new MarkdownPipelineBuilder()
                .UsePreciseSourceLocation()
                .Build();
            return Markdig.Markdown.Parse(markdown, pipeline);
        }

        private List<TimelineEventDto> ExtractTimelineEvents(MarkdownDocument document)
        {
            var events = new List<TimelineEventDto>();

            for (int i = 0; i < document.Count; i++)
            {
                var node = document[i];
                if (node is HeadingBlock heading && heading.Level == 6)
                {
                    var version = ExtractVersion(heading);
                    var tasks = ExtractTasks(document, ref i);

                    events.Add(new TimelineEventDto
                    {
                        Version = version,
                        Tasks = tasks
                    });
                }
            }

            return events;
        }

        private string ExtractVersion(HeadingBlock heading)
        {
            return heading.Inline.FirstChild.ToString();
        }

        private List<TimelineTaskDto> ExtractTasks(MarkdownDocument document, ref int currentIndex)
        {
            var tasks = new List<TimelineTaskDto>();

            for (int j = currentIndex + 1; j < document.Count; j++)
            {
                var sibling = document[j];
                if (sibling is HeadingBlock siblingHeading && siblingHeading.Level == 6)
                {
                    currentIndex = j - 1; // Update current index to the last processed node
                    break; // Exit the loop if the next version heading is found
                }
                if (sibling is ListBlock listBlock)
                {
                    tasks.AddRange(ParseListBlock(listBlock));
                }
            }

            return tasks;
        }

        private IEnumerable<TimelineTaskDto> ParseListBlock(ListBlock listBlock)
        {
            var tasks = new List<TimelineTaskDto>();

            foreach (var listItem in listBlock)
            {
                if (listItem is ListItemBlock listItemBlock)
                {
                    tasks.AddRange(ParseListItemBlock(listItemBlock));
                }
            }

            return tasks;
        }

        private IEnumerable<TimelineTaskDto> ParseListItemBlock(ListItemBlock listItemBlock)
        {
            var tasks = new List<TimelineTaskDto>();

            string Title = "";
            bool IsCompleted = false;
            List<string> SubTasks = new List<string>();

            foreach (var innerBlock in listItemBlock)
            {
                if (innerBlock is ParagraphBlock paragraphBlock)
                {
                    Title = sourceMarkdown.Substring(paragraphBlock.Span.Start, paragraphBlock.Span.Length);                    
                    if (Title.Contains("[x]"))
                    {
                        IsCompleted = true;
                    }

                    Title = Title.Replace("*", "").Replace("[ ]", "").Replace("[]", "").Replace("[x]", "").Trim();
                }

                if (innerBlock is ListBlock subListBlock)
                {
                    foreach (var subInnerListBlock in subListBlock) 
                    {
                        if (subInnerListBlock is ListItemBlock subListItemBlock)
                        {
                            foreach (var subInnerParagraphBlock in subListItemBlock)
                            {
                                if (subInnerParagraphBlock is ParagraphBlock subParagraphBlock)
                                {
                                    Title = sourceMarkdown.Substring(subParagraphBlock.Span.Start, subParagraphBlock.Span.Length);

                                    SubTasks.Add(Title.Replace("*", "").Trim());
                                }
                            }
                        }
                    }
                }
            }
            tasks.Add(new TimelineTaskDto
            {
                Title = Title,
                IsCompleted = IsCompleted,
                SubTasks = SubTasks
            });


            return tasks;
        }
    }
}
