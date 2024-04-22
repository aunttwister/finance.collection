using IntrinsicValue.Calculation.DataSets.DCFIntrinsicModel;
using IntrinsicValue.Calculation.DCFIntrinsicModel.Commands;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace IntrinsicValue.Calculation.DCFIntrinsicModel
{
    public class DCFIntrinsicModelService : ICalculateIntrinsicServiceStrategy<DCFIntrinsicModelCommand, DCFIntrinsicModelDataSet>
    {
        public DCFIntrinsicModelDataSet Calculate(DCFIntrinsicModelCommand request)
        {
            IDictionary<string, decimal> historicalCashFlow = request.HistoricalCashFlow;
            List<HistoricalGrowthRateDataSet> historicalGrowthRates = CalculateHistoricalGrowthRateTotal(historicalCashFlow);

            IEnumerable<string> years = historicalGrowthRates.Select(x => x.Year);
            IEnumerable<decimal> growthRates = historicalGrowthRates.Where(x => x.GrowthRate is not null)
                                                                    .Select(x => x.GrowthRate.Value);

            AverageGrowthRateDataSet averageGrowthRateDataSet = CalculateAverageGrowthRate(years, growthRates, request.SafetyMargin);

            decimal averageGrowthRate = averageGrowthRateDataSet.Value;
            decimal latestCashFlow = historicalCashFlow.First().Value;

            Dictionary<string, decimal> futureCashFlow = CalculateFutureCashFlow(averageGrowthRate, latestCashFlow);
            List<FutureCashFlowDataSet> futureCashFlowDataSet = PresentValueFacade(futureCashFlow);
            decimal sumFutureCashFlowValue = futureCashFlowDataSet.Select(fcf => fcf.PresentValue).Sum();

            decimal equityValue = CalculateEquityValue(sumFutureCashFlowValue, request.TTMCashAndCashEquivalents, request.TTMTotalDebt);

            decimal discountedCashFlowValue = CalculateDiscountedCashFlowValue(equityValue, request.SharesOutstanding);

            return new DCFIntrinsicModelDataSet(
                request,
                discountedCashFlowValue,
                equityValue,
                sumFutureCashFlowValue,
                futureCashFlowDataSet,
                averageGrowthRateDataSet,
                historicalGrowthRates);
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



        public AverageGrowthRateDataSet CalculateAverageGrowthRate(IEnumerable<string> years, IEnumerable<decimal> growthRates, decimal margin = 0.65m)
        {
            string firstYear = years.First();
            string lastYear = years.Last();

            decimal averageGrowthRate = growthRates.Average();

            return new AverageGrowthRateDataSet()
            {
                Period = firstYear + "-" + lastYear, 
                Value = Math.Round(averageGrowthRate * margin / 100, 2)
            };
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

        public List<FutureCashFlowDataSet> PresentValueFacade(Dictionary<string, decimal> yearCashFlowPairs, decimal discountRate = 0.08m)
        {
            List<FutureCashFlowDataSet> yearFutureCashFlow = new List<FutureCashFlowDataSet>();
            int multiplier = 1;
            foreach (var yearCashFlow in yearCashFlowPairs)
            {
                decimal presentValue = CalculatePresentValue(yearCashFlow.Value, discountRate, multiplier);
                FutureCashFlowDataSet yearlyFutureCashFlow = new FutureCashFlowDataSet()
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
