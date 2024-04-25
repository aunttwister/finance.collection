using Finance.Collection.Domain.IntrinsicValue.Calculation.DataSets.Base;
using Finance.Collection.Domain.IntrinsicValue.Calculation.DataSets.DCFIntrinsicModel;
using Finance.Collection.Domain.IntrinsicValue.Calculation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntrinsicValue.Calculation.DataSets.Results
{
    public class DCFCalculationResult : BaseIntrinsicModelDataSet, ICalculationResult
    {
        public DCFCalculationResult(
            string ticker,
            decimal currentPrice,
            decimal sharesOutstanding,
            ValuationDataSet valuationDataSet,
            AssetsDataSet assetsDataSet,
            LiabilitiesDataSet liabilitiesDataSet,
            CashFlowDataSet cashFlowDataSet,
            GrowthRateDataSet growthRateDataSet,
            ConfigurationDataSet configurationDataSet)
        {
            //Base Data
            Ticker = ticker;
            CurrentPrice = currentPrice;

            //Config params
            ConfigurationDataSet = configurationDataSet;

            //Specifics
            SharesOutstanding = sharesOutstanding;
            ValuationDataSet = valuationDataSet;
            AssetsDataSet = assetsDataSet;
            LiabilitiesDataSet = liabilitiesDataSet;
            CashFlowDataSet = cashFlowDataSet;
            GrowthRateDataSet = growthRateDataSet;

        }
        public decimal SharesOutstanding { get; set; }
        public CashFlowDataSet CashFlowDataSet { get; set; }
        public GrowthRateDataSet GrowthRateDataSet { get; set; }
        public AssetsDataSet AssetsDataSet { get; set; }
        public LiabilitiesDataSet LiabilitiesDataSet { get; set; }
        public ValuationDataSet ValuationDataSet { get; set; }
        public ConfigurationDataSet ConfigurationDataSet { get; set; }
    }
}
