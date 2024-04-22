using IntrinsicValue.Calculation.DataSets.Common;
using IntrinsicValue.Calculation.DataSets.GrahamIntrinsicModel;
using IntrinsicValue.Calculation.GrahamIntrinsicModel.Commands;

namespace IntrinsicValue.Calculation.GrahamIntrinsicModel
{
    public class GrahamIntrinsicModelService : ICalculateIntrinsicServiceStrategy<GrahamIntrinsicModelCommand, GrahamIntrinsicModelDataSet>
    {
        public GrahamIntrinsicModelDataSet Calculate(GrahamIntrinsicModelCommand request)
        {
            decimal intrinsicValue = Math.Round(request.Eps * (8.5m + 2m * request.FiveYearGrowth) * request.AverageBondYield / request.CurrentBondYield, 2);

            return new GrahamIntrinsicModelDataSet(request, intrinsicValue);
        }
    }
}