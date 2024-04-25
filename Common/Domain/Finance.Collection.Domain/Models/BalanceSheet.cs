using Finance.Collection.Domain.Common.Propagation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Collection.Domain.Models
{
    public class BalanceSheet : AuditableEntity
    {
        public decimal TTMCashEquivalents { get; set; }
        public decimal TTMTotalDebt { get; set; }
        public IDictionary<string, decimal> HistoricalCashEquivalents { get; set; }
        public IDictionary<string, decimal> HistoricalTotalDebt { get; set; }
    }
}
