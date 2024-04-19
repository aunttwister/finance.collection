﻿using MediatR;
using FinanceScraper.Common.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScraper.YCharts.TripleABondYieldScraper.Commands
{
    public class TripleABondYieldScraperCommand : YChartsTripleABondScraperBaseCommand, IRequest<TripleABondsDataSet>
    {
        public TripleABondYieldScraperCommand() : base() { }
    }
}
