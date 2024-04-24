using IntrinsicValue.Calculation.DataSets.Results;
using IntrinsicValue.Calculation.GrahamIntrinsicModel.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntrinsicValue.Calculation.DCFIntrinsicModel.Commands
{
    public class DCFIntrinsicModelCommandHandler : IRequestHandler<DCFIntrinsicModelCommand, DCFIntrinsicResult>
    {
        public readonly ICalculateIntrinsicServiceStrategy<DCFIntrinsicModelCommand, DCFIntrinsicResult> _calculateIntrinsicService;
        public DCFIntrinsicModelCommandHandler(ICalculateIntrinsicServiceStrategy<DCFIntrinsicModelCommand, DCFIntrinsicResult> calculateIntrinsicService)
        {
            _calculateIntrinsicService = calculateIntrinsicService;
        }
        public Task<DCFIntrinsicResult> Handle(DCFIntrinsicModelCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_calculateIntrinsicService.Calculate(request));
        }
    }
}
