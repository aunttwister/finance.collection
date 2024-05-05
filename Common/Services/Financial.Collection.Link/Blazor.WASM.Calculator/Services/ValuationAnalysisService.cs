using AutoMapper;
using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.FinanceScraper.Results;
using Finance.Collection.Domain.IntrinsicValue.Calculation.Results;
using FinanceScraper.Common.Init.Commands;
using Financial.Collection.Domain.DTOs;
using Financial.Collection.Link.Exceptions;
using Financial.Collection.Link.FinanceScraper.Encapsulation;
using Financial.Collection.Link.FinanceScraper.Services;
using Financial.Collection.Link.IntrinsicValue.Calculation.Services;
using IntrinsicValue.Calculation.Init.Commands;
using System.Reflection;

namespace Financial.Collection.Link.Blazor.WASM.Calculator.Services
{
    public class ValuationAnalysisService : IValuationAnalysisService
    {
        private readonly IFinanceScraperService _financeScraperService;
        private readonly IValueCalculationService _valueCalculationService;
        private readonly IMapper _mapper;

        public ValuationAnalysisService(
            IFinanceScraperService financeScraperService,
            IValueCalculationService valueCalculationService,
            IMapper mapper)
        {
            _financeScraperService = financeScraperService;
            _valueCalculationService = valueCalculationService;
            _mapper = mapper;
        }

        public async Task<MethodResultExceptionSum<TickerDto>> PerformAnalysis(ScraperParameterEncapsulator parameters)
        {
            InitScrapeCommand requestScrape = CreateInitScrapeCommand(parameters);

            MethodResult<IScrapeResult> scrapeResult = await _financeScraperService.ScrapeFinancialDataAsync(requestScrape);

            List<Exception> scrapeExceptions = ExceptionExtractor.ExtractExceptions(scrapeResult);

            TickerDto tickerDto = _mapper.Map<TickerDto>(scrapeResult.Data);

            AAABondDto aaaBondDto = _mapper.Map<AAABondDto>(scrapeResult.Data);

            if (scrapeExceptions.Count > 0)
                return new MethodResultExceptionSum<TickerDto>(tickerDto, scrapeExceptions);

            InitCalculationCommand requestCalc = PrepareCalculationCommand(scrapeResult.Data, tickerDto, aaaBondDto, parameters.SafetyMargin);

            MethodResult<ICalculationResult> calcResult = await _valueCalculationService.CalculateFinancialDataAsync(requestCalc);

            _mapper.Map(calcResult.Data, tickerDto);

            return new MethodResultExceptionSum<TickerDto>(tickerDto, scrapeExceptions);
        }

        public InitScrapeCommand CreateInitScrapeCommand(ScraperParameterEncapsulator encapsulator)
        {
            return _mapper.Map<InitScrapeCommand>(encapsulator);
        }

        public InitCalculationCommand PrepareCalculationCommand(IScrapeResult result, TickerDto tickerDto, AAABondDto aaaBondDto, decimal safetyMargin)
        {
            var context = new MapperConfiguration(cfg => {
                cfg.AddMaps(Assembly.GetExecutingAssembly());
            }).CreateMapper();

            InitCalculationCommand request = context.Map<InitCalculationCommand>(result);
            request.SafetyMargin = safetyMargin;
            request = context.Map(tickerDto, request);
            request = context.Map(aaaBondDto, request);

            return request;
        }
    }
}
