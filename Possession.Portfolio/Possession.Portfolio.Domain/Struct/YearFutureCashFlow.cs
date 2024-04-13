using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPortfolio.Domain.Struct
{
    public struct YearFutureCashFlow
    {
        public YearFutureCashFlow(string year, decimal cashFlow, decimal presentValue)
        {
            Year = year;
            CashFlow = cashFlow;
            PresentValue = presentValue;
        }
        public string Year {  get; set; }
        public decimal CashFlow { get; set; }
        public decimal PresentValue {  get; set; }

        public static decimal CalculatePresentValue(decimal cashFlow, decimal discountRate, int multiplier)
        {
            return (decimal)Math.Pow(Convert.ToDouble(cashFlow / (1 + discountRate)), multiplier);
        }
    }
}
