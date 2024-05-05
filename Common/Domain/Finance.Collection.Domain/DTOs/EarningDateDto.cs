namespace Financial.Collection.Domain.DTOs
{
    public class EarningDateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public TickerDto Ticker { get; set; }
    }
}
