using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudBlazor.Markdown.Extensions.Domain.DTOs
{
    public class MarkdownInfoDto
    {
        public string OriginalName { get; set; }
        public string DisplayName { get; set; }
        public int Priority { get; set; }
        public string UrlPath { get; set; }
        public string DirectoryPath { get; set; }
    }
}
