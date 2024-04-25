using Finance.Collection.Domain.IntrinsicValue.Calculation.DataSets.DCFIntrinsicModel;
using IntrinsicValue.Calculation.DataSets.DCFIntrinsicModel;
using IntrinsicValue.Calculation.DataSets.Results;
using IntrinsicValue.Calculation.DCFIntrinsicModel.Commands;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace IntrinsicValue.Calculation.DCFIntrinsicModel
{
    public class DCFIntrinsicModelService : ICalculateIntrinsicServiceStrategy<DCFIntrinsicModelCommand, DCFCalculationResult>
    {
        public DCFCalculationResult Calculate(DCFIntrinsicModelCommand request)
        {
            AssetsDataSet assetsDataSet = new AssetsDataSet(request.TTMCashAndCashEquivalents, request.HistoricalCashAndCashEquivalents);
            LiabilitiesDataSet liabilitiesDataSet = new LiabilitiesDataSet(request.TTMTotalDebt, request.HistoricalTotalDebt);
            ConfigurationDataSet configurationDataSet = new ConfigurationDataSet(request.DiscountRate, request.PerpetualRate, request.SafetyMargin);

            IDictionary<string, decimal> historicalCashFlow = request.HistoricalCashFlow;
            List<HistoricalGrowthRateDataSet> historicalGrowthRates = CalculateHistoricalGrowthRateTotal(historicalCashFlow);

            IEnumerable<string> years = historicalGrowthRates.Select(x => x.Year);
            IEnumerable<decimal> growthRates = historicalGrowthRates.Where(x => x.GrowthRate is not null)
                                                                    .Select(x => x.GrowthRate.Value);

            (string, decimal) averageGrowthRate = CalculateAverageGrowthRate(years, growthRates, request.SafetyMargin);

            decimal latestCashFlow = historicalCashFlow.First().Value;

            GrowthRateDataSet growthRateDataSet = new GrowthRateDataSet()
            {
                Period = averageGrowthRate.Item1,
                AverageGrowthRate = averageGrowthRate.Item2,
                HistoricalGrowthRate = historicalGrowthRates
            };

            Dictionary<string, decimal> futureCashFlow = CalculateFutureCashFlow(averageGrowthRate.Item2, latestCashFlow);
            List<FutureCashFlowPresentValueDataSet> futureCashFlowPresentValue = PresentValueFacade(futureCashFlow);
            decimal presentValueSum = futureCashFlowPresentValue.Select(fcf => fcf.PresentValue).Sum();

            CashFlowDataSet cashFlowDataSet = new CashFlowDataSet()
            {
                FutureCashFlowPresentValue = futureCashFlowPresentValue,
                HistoricalCashFlow = historicalCashFlow,
                PresentValueSum = presentValueSum
            };

            decimal equityValue = CalculateEquityValue(presentValueSum, request.TTMCashAndCashEquivalents, request.TTMTotalDebt);

            decimal discountedCashFlowValue = CalculateDiscountedCashFlowValue(equityValue, request.SharesOutstanding);

            ValuationDataSet valuationDataSet = new ValuationDataSet(equityValue, discountedCashFlowValue);

            return new DCFCalculationResult(
                request.Ticker,
                request.CurrentPrice,
                request.SharesOutstanding,
                valuationDataSet,
                assetsDataSet,
                liabilitiesDataSet,
                cashFlowDataSet,
                growthRateDataSet,
                configurationDataSet);
        }
        public List<HistoricalGrowthRateDataSet> CalculateHistoricalGrowthRateTotal(IDictionary<string, decimal> HistoricalCashFlows)
        {
            List<HistoricalGrowthRateDataSet> historicalGrowthRateDataSet = new List<HistoricalGrowthRateDataSet>();

            KeyValuePair<string, decimal> initialCashFlow = HistoricalCashFlows.Last();
            historicalGrowthRateDataSet.Add(new HistoricalGrowthRateDataSet(initialCashFlow.Key, initialCashFlow.Value));

            for (int index = HistoricalCashFlows.Count - 1; index > 0; index--)
            {
                KeyValuePair<string, decimal> currentCashFlow = HistoricalCashFlows.ElementAt(index - 1);
                decimal previousCashFlow = HistoricalCashFlows.ElementAt(index).Value;

                decimal growthRate = CalculateHistoricalGrowthRateSingle(currentCashFlow.Value, previousCashFlow);

                historicalGrowthRateDataSet.Add(new HistoricalGrowthRateDataSet(currentCashFlow.Key, currentCashFlow.Value, growthRate));
            }

            return historicalGrowthRateDataSet;
        }
        public decimal CalculateHistoricalGrowthRateSingle(decimal currentYearCashFlow, decimal previousYearCashFlow) =>
            Math.Round((currentYearCashFlow - previousYearCashFlow) / previousYearCashFlow * 100, 2);



        public (string, decimal) CalculateAverageGrowthRate(IEnumerable<string> years, IEnumerable<decimal> growthRates, decimal margin = 0.65m)
        {
            string firstYear = years.First();
            string lastYear = years.Last();

            decimal averageGrowthRate = growthRates.Average();

            string period = firstYear + "-" + lastYear;
            decimal value = Math.Round(averageGrowthRate * margin / 100, 2);
            
            return (period, value);
        }

        public Dictionary<string, decimal> CalculateFutureCashFlow(decimal averageGrowthRate, decimal cashFlow, int numberOfYears = 10)
        {
            int year = DateTime.Today.Year;

            Dictionary<string, decimal> yearGrowthPairs = new Dictionary<string, decimal>();

            for (int i = 1; i < numberOfYears; i++)
            {
                cashFlow = Math.Round(cashFlow * (1 + averageGrowthRate), 2);
                yearGrowthPairs.Add(year.ToString(), cashFlow);

                year++;
            }

            return yearGrowthPairs;
        }

        public decimal CalculateTerminalValue(decimal lastYearCashFlow, decimal perpetualRate = 2.5m, decimal discountRate = 8m) =>
            lastYearCashFlow * (1 + perpetualRate) / (discountRate - perpetualRate);

        public List<FutureCashFlowPresentValueDataSet> PresentValueFacade(Dictionary<string, decimal> yearCashFlowPairs, decimal discountRate = 0.08m)
        {
            List<FutureCashFlowPresentValueDataSet> yearFutureCashFlow = new List<FutureCashFlowPresentValueDataSet>();
            int multiplier = 1;
            foreach (var yearCashFlow in yearCashFlowPairs)
            {
                decimal presentValue = CalculatePresentValue(yearCashFlow.Value, discountRate, multiplier);
                FutureCashFlowPresentValueDataSet yearlyFutureCashFlow = new FutureCashFlowPresentValueDataSet()
                {
                    Year = yearCashFlow.Key,
                    CashFlow = yearCashFlow.Value,
                    PresentValue = presentValue,
                };
                yearFutureCashFlow.Add(yearlyFutureCashFlow);

                multiplier++;
            }

            return yearFutureCashFlow;
        }

        public decimal CalculatePresentValue(decimal cashFlow, decimal discountRate, int multiplier)
        {
            double converted = Convert.ToDouble(1m + discountRate);
            decimal divider = Convert.ToDecimal(Math.Pow(converted, multiplier));

            return Math.Round(cashFlow / divider, 2); ;
        }

        public decimal CalculateEquityValue(decimal sumPresentFutureValue, decimal cashAndCashEquivalents, decimal totalDebt) =>
            sumPresentFutureValue + cashAndCashEquivalents - totalDebt;

        public decimal CalculateDiscountedCashFlowValue(decimal equityValue, decimal sharesOutstanding) =>
            equityValue / sharesOutstanding;
    }
}
