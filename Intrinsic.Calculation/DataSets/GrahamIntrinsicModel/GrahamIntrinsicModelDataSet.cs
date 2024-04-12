﻿using Calculation.Intrinsic.DataSets.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculation.Intrinsic.DataSets.GrahamIntrinsicModel
{
    public class GrahamIntrinsicModelDataSet : BaseIntrinsicModelDataSet
    {
        public decimal Eps { get; set; }
        public decimal FiveYearGrowth { get; set; }
        public decimal AverageBondYield { get; set; }
        public decimal CurrentBondYield { get; set; }
        public IntrinsicValueDataSet IntrinsicValue { get; set; }
    }
}
