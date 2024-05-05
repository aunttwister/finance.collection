namespace Financial.Collection.Domain.DTOs
{
    public class HistoricalTickerIntrinsicValueDto
    {
        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
        public decimal IntrinsicValue { get; set; }
        public decimal SafetyMargin { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal PriceDifference { get; set; }
        public decimal PriceDifferencePercentage { get; set; }
        public string IntrinsicValueType { get; set; }
        public TickerDto Ticker { get; set; }
    }
}
