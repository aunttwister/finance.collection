namespace Financial.Collection.Domain.DTOs
{
    public class TickerListInstanceDto
    {
        public Guid Id { get; set; }
        public TickerListDto TickerListDto { get; set; }
        public TickerDto TickerDto { get; set; }
        public PortfolioSpecificDto PortfolioSpecific {  get; set; }
    }
}
