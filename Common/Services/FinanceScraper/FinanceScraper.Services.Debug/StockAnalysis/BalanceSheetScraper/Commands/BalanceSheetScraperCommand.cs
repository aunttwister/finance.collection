﻿using MediatR;
using FinanceScraper.Common.Base;
using FinanceScraper.Common.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScraper.StockAnalysis.BalanceSheetScraper.Commands
{
    public class BalanceSheetScraperCommand : StockAnalysisScraperBaseCommand, IRequest<BalanceSheetDataSet>
    {
        public BalanceSheetScraperCommand(string ticker, string path) : base(ticker, path) { }
    }
}
