using IntrinsicValue.Calculation.DataSets.Results;
using MediatR;

namespace IntrinsicValue.Calculation.GrahamIntrinsicModel.Commands
{
    public class GrahamIntrinsicModelCommandHandler : IRequestHandler<GrahamIntrinsicModelCommand, GrahamIntrinsicResult>
    {
        public readonly ICalculateIntrinsicServiceStrategy<GrahamIntrinsicModelCommand, GrahamIntrinsicResult> _calculateIntrinsicService;
        public GrahamIntrinsicModelCommandHandler(ICalculateIntrinsicServiceStrategy<GrahamIntrinsicModelCommand, GrahamIntrinsicResult> calculateIntrinsicService)
        {
            _calculateIntrinsicService = calculateIntrinsicService;
        }
        public Task<GrahamIntrinsicResult> Handle(GrahamIntrinsicModelCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_calculateIntrinsicService.Calculate(request));
        }
    }
}
