using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculation.Intrinsic
{
    public interface ICalculateIntrinsicServiceStrategy<in TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public TResponse Calculate(TRequest request);
    }
}
