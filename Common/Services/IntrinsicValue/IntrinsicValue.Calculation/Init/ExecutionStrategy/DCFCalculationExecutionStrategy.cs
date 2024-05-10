using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.FinanceScraper.Constants;
using Finance.Collection.Domain.FinanceScraper.Results;
using Finance.Collection.Domain.IntrinsicValue.Calculation.Results;
using Financial.Collection.Domain.DTOs;
using IntrinsicValue.Calculation.DataSets.Results;
using IntrinsicValue.Calculation.DCFIntrinsicModel.Commands;
using MediatR;

namespace IntrinsicValue.Calculation.Init.ExecutionStrategy
{
    public class DCFCalculationExecutionStrategy : ICalculationExecutionStrategy
    {
        private readonly IMediator _mediator;
        private readonly string _symbol;
        /*public DCFCalculationExecutionStrategy(IMediator mediator)
        {
            _mediator = mediator;
        }*/
        public DCFCalculationExecutionStrategy(IMediator mediator, string symbol)
        {
            _mediator = mediator;
            _symbol = symbol;
        }

        public async Task<MethodResult<ICalculationResult>> ExecuteCalculationStrategy(TickerDto tickerDto, AAABondDto aaaBondDto, decimal safetyMargin)
        {
            DCFIntrinsicModelCommand request = new DCFIntrinsicModelCommand(tickerDto, safetyMargin);

            Task<DCFCalculationResult> result = _mediator.Send(request);

            await result.ConfigureAwait(false);

            return new MethodResult<ICalculationResult>(result.Result);
        }
    }
}
