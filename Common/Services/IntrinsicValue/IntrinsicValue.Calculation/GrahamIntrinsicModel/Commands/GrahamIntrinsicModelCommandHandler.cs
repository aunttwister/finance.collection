using IntrinsicValue.Calculation.DataSets.GrahamIntrinsicModel;
using MediatR;

namespace IntrinsicValue.Calculation.GrahamIntrinsicModel.Commands
{
    public class GrahamIntrinsicModelCommandHandler : IRequestHandler<GrahamIntrinsicModelCommand, GrahamIntrinsicModelDataSet>
    {
        public readonly ICalculateIntrinsicServiceStrategy<GrahamIntrinsicModelCommand, GrahamIntrinsicModelDataSet> _calculateIntrinsicService;
        public GrahamIntrinsicModelCommandHandler(ICalculateIntrinsicServiceStrategy<GrahamIntrinsicModelCommand, GrahamIntrinsicModelDataSet> calculateIntrinsicService)
        {
            _calculateIntrinsicService = calculateIntrinsicService;
        }
        public Task<GrahamIntrinsicModelDataSet> Handle(GrahamIntrinsicModelCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_calculateIntrinsicService.Calculate(request));
        }
    }
}
