using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntrinsicValue.Calculation
{
    public class BaseIntrinsicModelCommand
    {
        public BaseIntrinsicModelCommand(string symbol, decimal currentPrice)
        {
            Symbol = symbol;
            CurrentPrice = currentPrice;
        }
        public string Symbol {  get; set; }
        public decimal CurrentPrice { get; set; }
    }
}
