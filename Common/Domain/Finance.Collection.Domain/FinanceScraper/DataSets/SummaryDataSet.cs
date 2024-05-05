﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.FinanceScraper.DataSets.Base;

namespace Finance.Collection.Domain.FinanceScraper.DataSets
{
    public class SummaryDataSet : IFinanceDataSet
    {
        public MethodResult<decimal> Eps { get; set; }
    }
}
