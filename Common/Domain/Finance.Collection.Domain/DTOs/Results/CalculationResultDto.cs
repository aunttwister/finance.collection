using Financial.Collection.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Collection.Domain.DTOs.Results
{
    public class CalculationResultDto : IResultDto
    {
        public CalculationResultDto(TickerDto tickerDto, AAABondDto aaaBondDto, List<Exception> exceptions)
        {
            TickerDto = tickerDto;
            AAABondDto = aaaBondDto;
            Exceptions = exceptions;
        }
        public TickerDto TickerDto { get; set; }
        public AAABondDto AAABondDto { get; set; }
        public List<Exception> Exceptions { get; set; }
    }
}
