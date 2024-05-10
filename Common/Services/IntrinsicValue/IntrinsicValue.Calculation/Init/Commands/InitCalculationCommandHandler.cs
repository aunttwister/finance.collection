using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.FinanceScraper.Results;
using Finance.Collection.Domain.IntrinsicValue.Calculation.Results;
using IntrinsicValue.Calculation.Init.ExecutionStrategy;
using IntrinsicValue.Calculation.Init.ExecutionStrategy.Factory;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntrinsicValue.Calculation.Init.Commands
{
    public class InitCalculationCommandHandler : IRequestHandler<InitCalculationCommand, MethodResult<ICalculationResult>>
    {
        private ICalculationExecutionStrategy _executionStrategy;
        private readonly IMediator _mediator;
        public InitCalculationCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<MethodResult<ICalculationResult>> Handle(InitCalculationCommand request, CancellationToken cancellationToken)
        {
            MethodResult<ICalculationExecutionStrategy> strategyResolveResult = SwitchStrategy(request.CalculationExecutionTypes, _mediator, request.TickerDto.Symbol);
            if (!strategyResolveResult.IsSuccessful)
            {
                return new MethodResult<ICalculationResult>(
                    null,
                    strategyResolveResult.Exception);
            }
            _executionStrategy = strategyResolveResult.Data;

            return await _executionStrategy.ExecuteCalculationStrategy(request.TickerDto, request.AAABondDto, request.SafetyMargin);
        }

        private MethodResult<ICalculationExecutionStrategy> SwitchStrategy(IEnumerable<Type> scrapeTypes, IMediator _mediator, string symnbol)
        {
            var factory = new CalculationExecutionStrategyFactory(_mediator, symnbol);
            return factory.GetCalculationExecutionStrategy(scrapeTypes);
        }
    }
}
