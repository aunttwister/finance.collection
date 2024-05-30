using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudBlazor.Markdown.Extensions.Domain.DTOs
{
    public class HeadingDto
    {
        public string Heading { get; set; }
        public int Indent { get; set; }
        public List<HeadingDto> NestedHeadings { get; set; } = new();
    }
}
