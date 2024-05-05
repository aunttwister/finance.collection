namespace Financial.Collection.Domain.DTOs
{
    public class NoteDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public bool IsPublic { get; set; }
        public TickerDto Ticker { get; set; }
        public UserDto User { get; set; }
    }
}
