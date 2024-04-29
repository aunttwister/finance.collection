using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.FinanceScraper.Results;
using Finance.Collection.Domain.IntrinsicValue.Calculation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntrinsicValue.Calculation.Init.Commands
{
    public class InitCalculationCommand : IRequest<MethodResult<ICalculationResult>>
    {
        public string Ticker { get; set; }
        public IScrapeResult ScrapeResult { get; set; }
        public decimal SafetyMargin { get; set; }
    }
}
