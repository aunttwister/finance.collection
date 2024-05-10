using Financial.Collection.Link.FinanceScraper.Encapsulation;
using Financial.Collection.Link.IntrinsicValue.Calculation.Encapsulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Collection.Link.Blazor.WASM.Calculator.Encapsulation
{
    public class ParameterEncapsulator
    {
        public ScraperParameterEncapsulator ScraperParameterEncapsulator { get; set; }
        public CalculationParameterEncapsulator CalculationParameterEncapsulator { get; set; }
        public bool CheckScrapeResult { get; set; }
    }
}
