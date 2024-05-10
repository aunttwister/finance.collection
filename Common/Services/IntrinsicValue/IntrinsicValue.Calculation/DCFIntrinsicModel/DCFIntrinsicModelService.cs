using Finance.Collection.Domain.IntrinsicValue.Calculation.Results.DCFIntrinsicModel;
using IntrinsicValue.Calculation.Common.Services;
using IntrinsicValue.Calculation.DataSets.Results;
using IntrinsicValue.Calculation.DCFIntrinsicModel.Commands;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace IntrinsicValue.Calculation.DCFIntrinsicModel
{
    public class DCFIntrinsicModelService : ICalculateIntrinsicServiceStrategy<DCFIntrinsicModelCommand, DCFCalculationResult>
    {
        private readonly IPostValuationService _postValuationService;
        public DCFIntrinsicModelService(IPostValuationService postValuationService)
        {
            _postValuationService = postValuationService;
        }
        public DCFCalculationResult Calculate(DCFIntrinsicModelCommand request)
        {
            IDictionary<string, decimal> historicalCashFlow = request.HistoricalCashFlow;
            Dictionary<string, decimal> historicalGrowthRates = CalculateHistoricalGrowthRateTotal(historicalCashFlow);

            IEnumerable<string> years = historicalGrowthRates.Keys;
            IEnumerable<decimal> growthRates = historicalGrowthRates.Values;

            var (averageGrowthRatePeriod, averageGrowthRate) = CalculateAverageGrowthRate(years, growthRates);

            HistoricalGrowthRateResultDataSet historicalGrowthRate = new HistoricalGrowthRateResultDataSet()
            {
                AveragePeriod = averageGrowthRatePeriod,
                AverageGrowthRate = averageGrowthRate,
                SafetyAverageGrowthRate = averageGrowthRate * request.SafetyMargin,
                HistoricalGrowthRates = historicalGrowthRates
            };

            decimal latestCashFlow = historicalCashFlow.First().Value;

            Dictionary<string, decimal> futureCashFlow = CalculateFutureCashFlow(historicalGrowthRate.SafetyAverageGrowthRate, latestCashFlow);
            List<EstimatedCashFlowResultDataSet> estimatedCashFlows = PresentValueFacade(futureCashFlow);

            decimal presentEstimatedCFValueSum = estimatedCashFlows.Select(fcf => fcf.EstimatedPresentValue).Sum();
            decimal terminalValue = CalculateTerminalValue(estimatedCashFlows.Last().EstimatedCashFlow, request.PerpetualRate, request.DiscountRate);
            decimal presentTerminalValue = CalculatePresentValue(terminalValue, request.DiscountRate, estimatedCashFlows.Count);

            decimal presentValueSum = presentEstimatedCFValueSum + presentTerminalValue;


            decimal equityValue = CalculateEquityValue(presentValueSum, request.TTMCashAndCashEquivalents, request.TTMTotalDebt);

            decimal discountedCashFlowValue = CalculateDiscountedCashFlowValue(equityValue, request.SharesOutstanding);

            decimal buyPrice = _postValuationService.CalculateBuyPrice(discountedCashFlowValue, request.SafetyMargin);
            decimal priceDifference = _postValuationService.CalculatePriceDifference(buyPrice, request.CurrentPrice);
            decimal priceDifferencePercent = _postValuationService.CalculatePriceDifferencePercent(priceDifference, request.CurrentPrice);

            return new DCFCalculationResult(
                discountedCashFlowValue,
                buyPrice,
                priceDifference,
                priceDifferencePercent,
                equityValue,
                request.SafetyMargin,
                estimatedCashFlows,
                historicalGrowthRate);
        }
        public Dictionary<string, decimal> CalculateHistoricalGrowthRateTotal(IDictionary<string, decimal> HistoricalCashFlows)
        {
            Dictionary<string, decimal> historicalGrowthRateDataSet = new Dictionary<string, decimal>();

            KeyValuePair<string, decimal> initialCashFlow = HistoricalCashFlows.Last();
            historicalGrowthRateDataSet.Add(initialCashFlow.Key, 0m);

            for (int index = HistoricalCashFlows.Count - 1; index > 0; index--)
            {
                KeyValuePair<string, decimal> currentCashFlow = HistoricalCashFlows.ElementAt(index - 1);
                decimal previousCashFlow = HistoricalCashFlows.ElementAt(index).Value;

                decimal growthRate = CalculateHistoricalGrowthRateSingle(currentCashFlow.Value, previousCashFlow);

                historicalGrowthRateDataSet.Add(currentCashFlow.Key, growthRate);
            }

            return historicalGrowthRateDataSet;
        }
        public decimal CalculateHistoricalGrowthRateSingle(decimal currentYearCashFlow, decimal previousYearCashFlow)
        {
            decimal cashFlowDeduction;
            if (currentYearCashFlow > 0 && previousYearCashFlow > 0)
                cashFlowDeduction = currentYearCashFlow - previousYearCashFlow;
            else if (currentYearCashFlow > 0 && previousYearCashFlow < 0)
                cashFlowDeduction = currentYearCashFlow + Math.Abs(previousYearCashFlow);
            else if (currentYearCashFlow < 0 && previousYearCashFlow > 0)
                cashFlowDeduction = Math.Abs(currentYearCashFlow) - previousYearCashFlow;
            else // currentYearCashFlow < 0 && previousYearCashFlow < 0
                cashFlowDeduction = currentYearCashFlow + Math.Abs(previousYearCashFlow);

            decimal growthRate;
            if (cashFlowDeduction > 0 && previousYearCashFlow > 0) // auto current > prev
                growthRate = cashFlowDeduction / previousYearCashFlow;
            else if (cashFlowDeduction < 0 && previousYearCashFlow > 0) // auto prev > current
            {
                if (previousYearCashFlow < 0)
                    growthRate = cashFlowDeduction / previousYearCashFlow * -1;
                else
                    growthRate = cashFlowDeduction / previousYearCashFlow;
            }    
            else if (cashFlowDeduction < 0 && previousYearCashFlow < 0)
                growthRate = cashFlowDeduction / previousYearCashFlow * -1;
            else // cashFlowDeduction > 0 && previousYearCashFlow < 0 // auto current > prev
                growthRate = cashFlowDeduction / previousYearCashFlow * -1;

            return Math.Round(growthRate, 6);
        }       



        public (string, decimal) CalculateAverageGrowthRate(IEnumerable<string> years, IEnumerable<decimal> growthRates)
        {
            string firstYear = years.First();
            string lastYear = years.Last();

            decimal averageGrowthRate = growthRates.Where(gr => gr != 0).Average();

            string period = firstYear + "-" + lastYear;
            decimal value = Math.Round(averageGrowthRate, 4);
            
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

        public decimal CalculateTerminalValue(decimal lastYearCashFlow, decimal perpetualRate = 0.025m, decimal discountRate = 0.08m) =>
            lastYearCashFlow * (1 + perpetualRate) / (discountRate - perpetualRate);

        public List<EstimatedCashFlowResultDataSet> PresentValueFacade(Dictionary<string, decimal> yearCashFlowPairs, decimal perpetualRate = 0.025m, decimal discountRate = 0.08m)
        {
            List<EstimatedCashFlowResultDataSet> yearFutureCashFlow = new List<EstimatedCashFlowResultDataSet>();
            int multiplier = 1;
            foreach (var yearCashFlow in yearCashFlowPairs)
            {
                /*decimal presentValue;
                if (yearCashFlow.Key == yearCashFlowPairs.Last().Key)
                    presentValue = CalculateTerminalValue(yearCashFlow.Value, multiplier, perpetualRate, discountRate);
                else*/
                decimal presentValue = CalculatePresentValue(yearCashFlow.Value, discountRate, multiplier);
                EstimatedCashFlowResultDataSet yearlyFutureCashFlow = new EstimatedCashFlowResultDataSet()
                {
                    Year = yearCashFlow.Key,
                    EstimatedCashFlow = yearCashFlow.Value,
                    EstimatedPresentValue = presentValue,
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
            Math.Round(equityValue / sharesOutstanding, 2);
    }
}
