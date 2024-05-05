namespace Financial.Collection.Domain.DTOs
{
    public class TickerDto
    {
        public TickerDto()
        {
            TickerIntrinsicValues = new List<HistoricalTickerIntrinsicValueDto>();
        }
        public Guid Id { get; set; }
        public string Symbol {  get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal EPS { get; set; }
        public decimal PE { get; set; }
        public string AverageGrowthRatePeriod { get; set; }
        public decimal AverageGrowthRate {  get; set; }
        public long SharesOutstanding { get; set; }
        public decimal TTMCashAndCashEquivalents { get; set; }
        public decimal TTMTotalDebt { get; set; }
        public decimal ExpectedFiveYearGrowth { get; set; }
        public DateTime ScrapeDate { get; set; }
        public List<NoteDto> Notes { get; set; }
        public EarningDateDto EarningsDate { get; set; }
        public List<YearlyDataDto> YearlyData {  get; set; }
        public List<TickerListDto> Tickerlists { get; set; }
        public List<HistoricalTickerIntrinsicValueDto> TickerIntrinsicValues { get; set; }
    }
}
