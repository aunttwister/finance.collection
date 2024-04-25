using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.FinanceScraper.Results;
using Finance.Collection.Domain.IntrinsicValue.Calculation.Results;
using IntrinsicValue.Calculation.Init.ExecutionStrategy;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntrinsicValue.Calculation.Init.Commands
{
    public class InitCommandHandler : IRequestHandler<InitCommand, MethodResult<ICalculationResult>>
    {
        private ICalculationExecutionStrategy _executionStrategy;
        private readonly IMediator _mediator;
        public InitCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task<MethodResult<ICalculationResult>> Handle(InitCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private MethodResult<ICalculationExecutionStrategy> SwitchStrategy(InitCommand request, IMediator _mediator)
        {
            var factory = new CalculationExecutionStrategyFactory(_mediator, request.ScrapeResult);
            return factory.GetCalculationExecutionStrategy(request);
        }
    }
}
