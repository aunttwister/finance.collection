using IntrinsicValue.Calculation.Common.Services;
using IntrinsicValue.Calculation.DataSets.Common;
using IntrinsicValue.Calculation.DataSets.Results;
using IntrinsicValue.Calculation.GrahamIntrinsicModel.Commands;

namespace IntrinsicValue.Calculation.GrahamIntrinsicModel
{
    public class GrahamIntrinsicModelService : ICalculateIntrinsicServiceStrategy<GrahamIntrinsicModelCommand, GrahamCalculationResult>
    {
        private readonly IPostValuationService _postValuationService;
        public GrahamIntrinsicModelService(IPostValuationService postValuationService)
        {
            _postValuationService = postValuationService;
        }
        public GrahamCalculationResult Calculate(GrahamIntrinsicModelCommand request)
        {
            decimal intrinsicValue = Math.Round(request.Eps * (8.5m + 2m * request.FiveYearGrowth) * request.AverageBondYield / request.CurrentBondYield, 2);

            decimal buyPrice = _postValuationService.CalculateBuyPrice(intrinsicValue, request.SafetyMargin);
            decimal priceDifference = _postValuationService.CalculatePriceDifference(buyPrice, request.CurrentPrice);
            decimal priceDifferencePercent = _postValuationService.CalculatePriceDifferencePercent(priceDifference, request.CurrentPrice);

            return new GrahamCalculationResult(intrinsicValue, buyPrice, priceDifference, priceDifferencePercent, request.SafetyMargin);
        }
    }
}