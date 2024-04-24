﻿using FinanceScraper.Common.DataSets.Results;
using FinanceScraper.Common.Propagation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScraper.Common.Init.Commands
{
    public class InitCommand : IRequest<MethodResult<IScrapeResult>>
    {
        public string Ticker { get; set; }
        public bool ExecuteGrahamScrape { get; set; } = true;
        public bool ExecuteDCFScrape { get; set; }
    }
}
