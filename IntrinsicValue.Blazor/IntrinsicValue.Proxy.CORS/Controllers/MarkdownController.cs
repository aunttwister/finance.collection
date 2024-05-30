using Intrinsicly.Api.Services.Category;
using Intrinsicly.Api.Services.MarkdownButler;
using Intrinsicly.Api.Services.ReadWebContent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MudBlazor.Markdown.Extensions.Domain.DTOs;
using MudBlazor.Markdown.Extensions.MarkdownRoadmapGenerator;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Intrinsicly.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarkdownController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IReadWebContentService _readWebContentService;
        private readonly IMarkdownRoadmpaGeneratorService _roadmapGeneratorService;

        public MarkdownController(
            ICategoryService categoryService,
            IReadWebContentService readWebContentService,
            IMarkdownRoadmpaGeneratorService roadmapGeneratorService)
        {
            _categoryService = categoryService;
            _readWebContentService = readWebContentService;
            _roadmapGeneratorService = roadmapGeneratorService;
        }

        [HttpGet("files")]
        public IActionResult GetMarkdownFiles()
        {
            List<string> markdownFiles = _readWebContentService.GetMarkdownFiles();
            var result = _categoryService.CategorizeFiles(markdownFiles);

            if (result is null)
                return Ok("");

            return Ok(result);
        }

        [HttpGet("entity")]
        public IActionResult GetMarkdownEntity([FromQuery] string urlPath)
        {
            KeyValuePair<MarkdownInfoDto, string>? result = _readWebContentService.GetMarkdownEntity(urlPath);

            if (result is null)
                return NotFound("");

            return Ok(result.Value);
        }

        [HttpGet("content")]
        public IActionResult GetMarkdownContent([FromQuery] string urlPath)
        {
            var result = _readWebContentService.GetMarkdownContent(urlPath);

            if (result == "")
                return Ok("");

            return Ok(result);
        }

        [HttpGet("parseEntity")]
        public IActionResult GetParsedRoadmapAsync([FromQuery] string urlPath)
        {
            string markDownContent = _readWebContentService.GetMarkdownContent(urlPath);

            var result = _roadmapGeneratorService.ParseRoadmapMarkdown(markDownContent);

            if (result is null || result.Count < 1)
                return NotFound();

            return Ok(result);
        }
    }
}