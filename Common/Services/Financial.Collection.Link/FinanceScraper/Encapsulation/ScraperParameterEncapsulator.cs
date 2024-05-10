namespace Financial.Collection.Link.FinanceScraper.Encapsulation
{
    public class ScraperParameterEncapsulator
    {
        public string Ticker { get; set; }
        public bool ExecuteGrahamScrape { get; set; }
        public bool ExecuteDCFScrape { get; set; }
        public bool UseHtmlContent { get; set; }
    }
}
