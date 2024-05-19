using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudBlazor.Markdown.Extensions.Domain.DTOs
{
    public class TimelineEventDto
    {
        public string Version { get; set; }
        public List<TimelineTaskDto> Tasks { get; set; }
        public bool IsCompleted { get; set; }
    }
}
