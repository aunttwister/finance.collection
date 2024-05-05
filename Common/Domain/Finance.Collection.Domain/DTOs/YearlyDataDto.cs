namespace Financial.Collection.Domain.DTOs
{
    public class YearlyDataDto
    {
        public Guid Id { get; set; }
        public string Year { get; set; }
        public decimal GrowthRate { get; set; }
        public decimal CashFlow { get; set; }
        public decimal EstimatedCashFlow { get; set; }
        public decimal EstimatedPresentValue { get; set; }
        public decimal TotalDebt { get; set; }
        public decimal CashAndCashEquivalents { get; set; }
        public TickerDto Ticker { get; set; }
    }
}
