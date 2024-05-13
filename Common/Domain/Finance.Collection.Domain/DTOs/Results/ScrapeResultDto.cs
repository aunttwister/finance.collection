using Finance.Collection.Domain.FinanceScraper.Exceptions;
using Financial.Collection.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Collection.Domain.DTOs.Results
{
    public class ScrapeResultDto : IResultDto
    {
        public ScrapeResultDto(TickerDto tickerDto, AAABondDto aaaBondDto, List<Exception> exceptions)
        {
            TickerDto = tickerDto;
            AAABondDto = aaaBondDto;
            Exceptions = exceptions;
            TickerValidationSuccessful = exceptions.Any(ex => ex.GetType() != typeof(TickerValidationException));
            ResolveHtmlNodeSuccessful = exceptions.Any(ex => ex.GetType() != typeof(UnresolvableHtmlNodeException));
        }
        public TickerDto TickerDto { get; set; }
        public AAABondDto AAABondDto { get; set; }
        public List<Exception> Exceptions { get; set; }
        public bool TickerValidationSuccessful { get; set; }
        public bool ResolveHtmlNodeSuccessful { get; set; }
    }
}
