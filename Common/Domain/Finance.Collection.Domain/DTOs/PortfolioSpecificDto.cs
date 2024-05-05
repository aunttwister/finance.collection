namespace Financial.Collection.Domain.DTOs
{
    public class PortfolioSpecificDto
    {
        public Guid Id { get; set; }
        public decimal Position { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal Profit { get; set; }
        public decimal ProfitPercentage { get; set; }
        public TickerListInstanceDto TickerListInstance { get; set; }
    }
}
