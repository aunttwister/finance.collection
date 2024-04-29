namespace IntrinsicValue.Blazor.Model
{
    public class WatchlistDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<TickerDto> Tickers { get; set; }
        public bool IsPortfolio { get; set; }
    }
}
