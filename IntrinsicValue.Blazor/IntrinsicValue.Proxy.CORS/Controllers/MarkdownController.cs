using Intrinsicly.Api.Services.ReadWebContent;
using Intrinsicly.Api.Services.UnpackMarkdown;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MudBlazor.Markdown.Extensions.MarkdownRoadmapGeneratorService;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Intrinsicly.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarkdownController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IMarkdownButlerService _markdownButlerService;
        private readonly IReadWebContentService _readWebContentService;
        private readonly IMarkdownRoadmpaGeneratorService _roadmapGeneratorService;

        public MarkdownController(IWebHostEnvironment env, 
            IMarkdownButlerService markdownButlerService,
            IReadWebContentService readWebContentService,
            IMarkdownRoadmpaGeneratorService roadmapGeneratorService)
        {
            _env = env;
            _markdownButlerService = markdownButlerService;
            _readWebContentService = readWebContentService;
            _roadmapGeneratorService = roadmapGeneratorService;
        }

        [HttpGet("files")]
        public IActionResult GetMarkdownFiles()
        {
            List<string> markdownFiles = _readWebContentService.GetMarkdownFiles(_env);
            var result = _markdownButlerService.CategorizeMarkdownFiles(markdownFiles);

            if (result is null)
                return Ok("");

            return Ok(result);
        }

        [HttpGet("content")]
        public IActionResult GetMarkdownContent([FromQuery] string directoryPath)
        {
            var result = _readWebContentService.GetMarkdownContent(_env, directoryPath);

            if (result == "")
                return Ok("");

            return Ok(result);
        }

        [HttpGet("roadmap")]
        public IActionResult GetParsedRoadmapAsync([FromQuery] string directoryPath)
        {
            string roadMapMarkdown = _readWebContentService.GetMarkdownContent(_env, directoryPath);
            var result = _roadmapGeneratorService.ParseRoadmapMarkdown(roadMapMarkdown);

            if (result is null || result.Count < 1)
                return NotFound();

            return Ok(result);
        }
    }
}