namespace IntrinsicValue.Blazor.Model
{
    public class TickerDto
    {
        public Guid Id { get; set; }
        public string Ticker {  get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal EPS { get; set; }
        public decimal PE { get; set; }
        public long SharesOutstanding { get; set; }
        public decimal TTMCashAndCashEquivalents { get; set; }
        public decimal TTMTotalDebt { get; set; }
        public GrowthRateDto GrowthRate { get; set; }
        public EarningsDateDto EarningsDate { get; set; }
        public BenjaminGrahamModelDto BenjaminGrahamModel { get; set; }
        public DCFModelDto DCFModel { get; set; }
        public AverageIntrinsicDto AverageIntrinsic { get; set; }
        public ICollection<WatchlistDto> Watchlists { get; set; }
    }
}
