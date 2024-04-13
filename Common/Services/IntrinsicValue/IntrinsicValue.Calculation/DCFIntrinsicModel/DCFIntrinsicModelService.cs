using IntrinsicValue.Calculation.DataSets.DCFIntrinsicModel;
using IntrinsicValue.Calculation.DCFIntrinsicModel.Commands;
using System.Reflection.Metadata.Ecma335;

namespace IntrinsicValue.Calculation.DCFIntrinsicModel
{
    public class DCFIntrinsicModelService : ICalculateIntrinsicServiceStrategy<DCFIntrinsicModelCommand, DCFIntrinsicModelDataSet>
    {
        public DCFIntrinsicModelDataSet Calculate(DCFIntrinsicModelCommand request)
        {
            IDictionary<string, decimal> historicalCashFlows = request.HistoricalCashFlows;

            IDictionary<string, decimal> historicalGrowthRates = CalculateHistoricalGrowthRateTotal(historicalCashFlows);

            AverageGrowthRateDataSet averageGrowthRateDataSet = CalculateAverageGrowthRate(historicalGrowthRates);

            decimal averageGrowthRate = averageGrowthRateDataSet.Value;
            decimal latestCashFlow = historicalCashFlows.Last().Value;

            Dictionary<string, decimal> futureCashFlows = CalculateFutureCashFlow(averageGrowthRate, latestCashFlow);

            List<FutureCashFlowDataSet> futureCashFlowDataSet = PresentValueFacade(futureCashFlows);

            decimal sumPresentFutureValue = futureCashFlowDataSet.Select(fcf => fcf.PresentValue).Sum();

            decimal equityValue = CalculateEquityValue(sumPresentFutureValue, request.CashAndCashEquivalents, request.TotalDebt);

            decimal discountedCashFlowValue = CalculateDiscountedCashFlowValue(equityValue, request.SharesOutstanding);

            return new DCFIntrinsicModelDataSet()
            {
                DiscountedCashFlowValue = discountedCashFlowValue,
                EquityValue = equityValue,
                SharesOutstanding = request.SharesOutstanding,
                CashAndCashEquivalents = request.CashAndCashEquivalents,
                TotalDebt = request.TotalDebt,
                SumPresentFutureCashFlowValue = sumPresentFutureValue,
                FutureCashFlowDataSet = futureCashFlowDataSet,
                FutureCashFlows = futureCashFlows,
                AverageGrowthRateDataSet = averageGrowthRateDataSet,
                HistoricalGrowthRates = historicalGrowthRates,
                HistoricalCashFlows = historicalCashFlows,
                DiscountRate = 8m,
                PerpetualRate = 2.5m,
                CurrentPrice = request.CurrentPrice,
                Ticker = request.Ticker
            };
        }
        public IDictionary<string, decimal> CalculateHistoricalGrowthRateTotal(IDictionary<string, decimal> HistoricalCashFlows)
        {
            IDictionary<string, decimal> historicalGrowthRates = new Dictionary<string, decimal>();

            for (int index = 1; index <= HistoricalCashFlows.Count; index++)
            {
                decimal previousHistoricalCashFlow = HistoricalCashFlows.ElementAt(index - 1).Value;
                KeyValuePair<string, decimal> currentHistoricalCashFlow = HistoricalCashFlows.ElementAt(index);

                KeyValuePair<string, decimal> historicalGrowthRate = CalculateHistoricalGrowthRateSingle(currentHistoricalCashFlow.Key, currentHistoricalCashFlow.Value, previousHistoricalCashFlow);

                historicalGrowthRates.Add(historicalGrowthRate);
            }

            return historicalGrowthRates;
        }
        public KeyValuePair<string, decimal> CalculateHistoricalGrowthRateSingle(string currentYear, decimal currentYearCashFlow, decimal previousYearCashFlow)
        {
            decimal growthRate = (currentYearCashFlow - previousYearCashFlow) / previousYearCashFlow * 100;
            return new KeyValuePair<string, decimal>(currentYear, growthRate);
        }
        public AverageGrowthRateDataSet CalculateAverageGrowthRate(IDictionary<string, decimal> historicalCashFlow, decimal margin = 0.65m)
        {
            string firstYear = historicalCashFlow.Keys.First();
            string lastYear = historicalCashFlow.Keys.Last();

            decimal averageGrowthRate = historicalCashFlow.Values.Average();

            return new AverageGrowthRateDataSet()
            {
                Period = firstYear + "-" + lastYear, 
                Value = (averageGrowthRate * margin) / 100
            };
        }

        public Dictionary<string, decimal> CalculateFutureCashFlow(decimal averageGrowthRate, decimal initialCashFlow, int numberOfYears = 10)
        {
            int year = DateTime.Today.Year;

            Dictionary<string, decimal> yearGrowthPairs = new Dictionary<string, decimal>();

            for (int i = 1; i < numberOfYears; i++)
            {
                initialCashFlow = initialCashFlow * (1 + averageGrowthRate);
                yearGrowthPairs.Add(year.ToString(), initialCashFlow);

                year++;
            }

            return yearGrowthPairs;
        }

        public decimal CalculateTerminalValue(decimal lastYearCashFlow, decimal perpetualRate = 2.5m, decimal discountRate = 8m) =>
            lastYearCashFlow * (1 + perpetualRate) / (discountRate - perpetualRate);

        public List<FutureCashFlowDataSet> PresentValueFacade(Dictionary<string, decimal> yearCashFlowPairs, decimal discountRate = 8m)
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

        public decimal CalculatePresentValue(decimal cashFlow, decimal discountRate, int multiplier) =>
            (decimal)Math.Pow(Convert.ToDouble(cashFlow / (1 + discountRate)), multiplier);

        public decimal CalculateEquityValue(decimal sumPresentFutureValue, decimal cashAndCashEquivalents, decimal totalDebt) =>
            sumPresentFutureValue + cashAndCashEquivalents - totalDebt;

        public decimal CalculateDiscountedCashFlowValue(decimal equityValue, decimal sharesOutstanding) =>
            equityValue / sharesOutstanding;
    }
}
