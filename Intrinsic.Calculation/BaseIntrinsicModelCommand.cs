using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculation.Intrinsic
{
    public class BaseIntrinsicModelCommand
    {
        public BaseIntrinsicModelCommand(string ticker, decimal currentPrice)
        {
            Ticker = ticker;
            CurrentPrice = currentPrice;
        }
        public string Ticker {  get; set; }
        public decimal CurrentPrice { get; set; }
    }
}
