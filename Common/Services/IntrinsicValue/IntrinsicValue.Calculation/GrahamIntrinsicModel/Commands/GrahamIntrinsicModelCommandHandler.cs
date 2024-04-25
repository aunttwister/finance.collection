using IntrinsicValue.Calculation.DataSets.Results;
using MediatR;

namespace IntrinsicValue.Calculation.GrahamIntrinsicModel.Commands
{
    public class GrahamIntrinsicModelCommandHandler : IRequestHandler<GrahamIntrinsicModelCommand, GrahamCalculationResult>
    {
        public readonly ICalculateIntrinsicServiceStrategy<GrahamIntrinsicModelCommand, GrahamCalculationResult> _calculateIntrinsicService;
        public GrahamIntrinsicModelCommandHandler(ICalculateIntrinsicServiceStrategy<GrahamIntrinsicModelCommand, GrahamCalculationResult> calculateIntrinsicService)
        {
            _calculateIntrinsicService = calculateIntrinsicService;
        }
        public Task<GrahamCalculationResult> Handle(GrahamIntrinsicModelCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_calculateIntrinsicService.Calculate(request));
        }
    }
}
