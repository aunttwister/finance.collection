using IntrinsicValue.Calculation.DataSets.DCFIntrinsicModel;
using IntrinsicValue.Calculation.DataSets.GrahamIntrinsicModel;
using IntrinsicValue.Calculation.GrahamIntrinsicModel.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntrinsicValue.Calculation.DCFIntrinsicModel.Commands
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
