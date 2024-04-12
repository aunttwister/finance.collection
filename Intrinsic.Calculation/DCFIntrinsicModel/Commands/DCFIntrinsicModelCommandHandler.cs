using Calculation.Intrinsic.DataSets.DCFIntrinsicModel;
using Calculation.Intrinsic.DataSets.GrahamIntrinsicModel;
using Calculation.Intrinsic.GrahamIntrinsicModel.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculation.Intrinsic.DCFIntrinsicModel.Commands
{
    public class DCFIntrinsicModelCommandHandler : IRequestHandler<DCFIntrinsicModelCommand, DCFIntrinsicModelDataSet>
    {
        public readonly ICalculateIntrinsicServiceStrategy<DCFIntrinsicModelCommand, DCFIntrinsicModelDataSet> _calculateIntrinsicService;
        public DCFIntrinsicModelCommandHandler(ICalculateIntrinsicServiceStrategy<DCFIntrinsicModelCommand, DCFIntrinsicModelDataSet> calculateIntrinsicService)
        {
            _calculateIntrinsicService = calculateIntrinsicService;
        }
        public Task<DCFIntrinsicModelDataSet> Handle(DCFIntrinsicModelCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_calculateIntrinsicService.Calculate(request));
        }
    }
}
