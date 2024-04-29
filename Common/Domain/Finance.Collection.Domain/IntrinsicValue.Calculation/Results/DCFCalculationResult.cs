using Finance.Collection.Domain.IntrinsicValue.Calculation.DataSets.DCFIntrinsicModel;
using Finance.Collection.Domain.IntrinsicValue.Calculation.Results;
using Finance.Collection.Domain.IntrinsicValue.Calculation.Results.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntrinsicValue.Calculation.DataSets.Results
{
    public class DCFCalculationResult : BaseCalculationResult, ICalculationResult
    {
        public DCFCalculationResult(
            string ticker,
            decimal sharesOutstanding,
            ValuationDataSet valuationDataSet,
            AssetsDataSet assetsDataSet,
            LiabilitiesDataSet liabilitiesDataSet,
            CashFlowDataSet cashFlowDataSet,
            GrowthRateDataSet growthRateDataSet,
            ConfigurationDataSet configurationDataSet) : base(valuationDataSet.DiscountedCashFlowValue.Value)
        {
            Ticker = ticker;

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
        public DCFCalculationResult(
            string ticker,
            decimal currentPrice,
            decimal sharesOutstanding,
            ValuationDataSet valuationDataSet,
            AssetsDataSet assetsDataSet,
            LiabilitiesDataSet liabilitiesDataSet,
            CashFlowDataSet cashFlowDataSet,
            GrowthRateDataSet growthRateDataSet,
            ConfigurationDataSet configurationDataSet,
            decimal safetyMargin) : base(currentPrice, valuationDataSet.DiscountedCashFlowValue.Value, safetyMargin)
        {
            Ticker = ticker;

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
        public string Ticker { get; set; }
        public decimal SharesOutstanding { get; set; }
        public CashFlowDataSet CashFlowDataSet { get; set; }
        public GrowthRateDataSet GrowthRateDataSet { get; set; }
        public AssetsDataSet AssetsDataSet { get; set; }
        public LiabilitiesDataSet LiabilitiesDataSet { get; set; }
        public ValuationDataSet ValuationDataSet { get; set; }
        public ConfigurationDataSet ConfigurationDataSet { get; set; }
    }
}
