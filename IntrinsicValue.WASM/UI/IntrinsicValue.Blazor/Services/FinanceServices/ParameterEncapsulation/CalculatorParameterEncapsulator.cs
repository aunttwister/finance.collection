namespace IntrinsicValue.Blazor.Services.FinanceServices.Encapsulation
{
    public class CalculatorParameterEncapsulator
    {
        public string Ticker { get; set; }
        public bool ExecuteGrahamScrape { get; set; }
        public bool ExecuteDCFScrape { get; set; }
        public bool UseHtmlContent { get; set; } = true;
        public decimal SafetyMargin { get; set; }
    }
}
