using IntrinsicValue.Calculation.DataSets.Common;
using IntrinsicValue.Calculation.DataSets.Results;
using IntrinsicValue.Calculation.GrahamIntrinsicModel.Commands;

namespace IntrinsicValue.Calculation.GrahamIntrinsicModel
{
    public class GrahamIntrinsicModelService : ICalculateIntrinsicServiceStrategy<GrahamIntrinsicModelCommand, GrahamIntrinsicResult>
    {
        public GrahamIntrinsicResult Calculate(GrahamIntrinsicModelCommand request)
        {
            decimal intrinsicValue = Math.Round(request.Eps * (8.5m + 2m * request.FiveYearGrowth) * request.AverageBondYield / request.CurrentBondYield, 2);

            return new GrahamIntrinsicResult(request, intrinsicValue);
        }
    }
}