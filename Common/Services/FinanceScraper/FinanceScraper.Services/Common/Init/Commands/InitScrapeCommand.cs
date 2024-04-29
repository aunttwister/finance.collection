using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.FinanceScraper.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScraper.Common.Init.Commands
{
    public class InitScrapeCommand : IRequest<MethodResult<IScrapeResult>>
    {
        public string Ticker { get; set; }
        public bool ExecuteGrahamScrape { get; set; }
        public bool ExecuteDCFScrape { get; set; }
        public decimal SafetyMargin { get; set; }
    }
}
