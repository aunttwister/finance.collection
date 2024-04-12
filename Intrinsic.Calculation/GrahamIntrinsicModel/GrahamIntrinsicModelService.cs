using Calculation.Intrinsic.DataSets.Common;
using Calculation.Intrinsic.DataSets.GrahamIntrinsicModel;
using Calculation.Intrinsic.GrahamIntrinsicModel.Commands;

namespace Calculation.Intrinsic.GrahamIntrinsicModel
{
    public class GrahamIntrinsicModelService : ICalculateIntrinsicServiceStrategy<GrahamIntrinsicModelCommand, GrahamIntrinsicModelDataSet>
    {
        public GrahamIntrinsicModelDataSet Calculate(GrahamIntrinsicModelCommand request)
        {
            decimal intrinsicValue = Math.Round(request.Eps * (8.5m + 2m * request.FiveYearGrowth) * request.AverageBondYield / request.CurrentBondYield, 2);

            return new GrahamIntrinsicModelDataSet()
            {
                Eps = request.Eps,
                FiveYearGrowth = request.FiveYearGrowth,
                AverageBondYield = request.AverageBondYield,
                CurrentBondYield = request.CurrentBondYield,
                IntrinsicValue = new IntrinsicValueDataSet()
                {
                    Value = intrinsicValue,
                    Date = DateTime.Now.Date
                }
            };
        }
    }
}