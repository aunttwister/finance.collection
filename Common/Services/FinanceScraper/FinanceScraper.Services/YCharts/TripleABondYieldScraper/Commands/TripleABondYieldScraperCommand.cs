using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finance.Collection.Domain.FinanceScraper.DataSets;
using FinanceScraper.Common.NodeResolver;

namespace FinanceScraper.YCharts.TripleABondYieldScraper.Commands
{
    public class TripleABondYieldScraperCommand : YChartsTripleABondScraperBaseCommand, IRequest<TripleABondsDataSet>
    {
        public TripleABondYieldScraperCommand() : base() { }
    }
}
