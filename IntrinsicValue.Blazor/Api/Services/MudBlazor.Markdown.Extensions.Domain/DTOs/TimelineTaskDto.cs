namespace MudBlazor.Markdown.Extensions.Domain.DTOs
{
    public class TimelineTaskDto
    {
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
        public List<string> SubTasks { get; set; } = new List<string>();
    }
}
