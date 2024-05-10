using AutoMapper;
using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.DTOs.Results;
using Finance.Collection.Domain.FinanceScraper.Results;
using Finance.Collection.Domain.IntrinsicValue.Calculation.Requests.CalculationExecution;
using Finance.Collection.Domain.IntrinsicValue.Calculation.Results;
using FinanceScraper.Common.Init.Commands;
using Financial.Collection.Domain.DTOs;
using Financial.Collection.Link.Exceptions;
using Financial.Collection.Link.FinanceScraper.Encapsulation;
using Financial.Collection.Link.FinanceScraper.Services;
using Financial.Collection.Link.IntrinsicValue.Calculation.Encapsulator;
using Financial.Collection.Link.IntrinsicValue.Calculation.Services;
using IntrinsicValue.Calculation.Init.Commands;
using System.Collections.Generic;
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

        public async Task<ScrapeResultDto> PerformScrape(ScraperParameterEncapsulator parameters)
        {
            InitScrapeCommand requestScrape = CreateInitScrapeCommand(parameters);
            MethodResult<IScrapeResult> scrapeResult = await _financeScraperService.ScrapeFinancialDataAsync(requestScrape);
            List<Exception> scrapeExceptions = ExceptionExtractor.ExtractExceptions(scrapeResult);

            TickerDto tickerDto = _mapper.Map<TickerDto>(scrapeResult.Data);
            AAABondDto aaaBondDto = _mapper.Map<AAABondDto>(scrapeResult.Data);

            return new ScrapeResultDto(tickerDto, aaaBondDto, scrapeExceptions); 
        }

        public async Task<CalculationResultDto> PerformValuation(CalculationParameterEncapsulator parameters)
        {
            parameters.SafetyMargin /= 100;

            InitCalculationCommand requestCalc = PrepareCalculationCommand(parameters);
            MethodResult<ICalculationResult> calcResult = await _valueCalculationService.CalculateFinancialDataAsync(requestCalc);

            TickerDto tickerDto = parameters.TickerDto;
            tickerDto = _mapper.Map(calcResult.Data, tickerDto);

            AAABondDto aaaBondDto = parameters.AAABondDto;
            List<Exception> exceptions = ExceptionExtractor.ExtractExceptions(calcResult);

            return new CalculationResultDto(tickerDto, aaaBondDto, exceptions);
        }

        public InitScrapeCommand CreateInitScrapeCommand(ScraperParameterEncapsulator parameters)
        {
            return _mapper.Map<InitScrapeCommand>(parameters);
        }

        public InitCalculationCommand PrepareCalculationCommand(CalculationParameterEncapsulator parameters)
        {
            var context = new MapperConfiguration(cfg => {
                cfg.AddMaps(Assembly.GetExecutingAssembly());
            }).CreateMapper();

            InitCalculationCommand request = new InitCalculationCommand();
            request.CalculationExecutionTypes = SwitchCalculationExecutionRequest(parameters);
            request.SafetyMargin = parameters.SafetyMargin;
            request.TickerDto = parameters.TickerDto;
            request.AAABondDto = parameters.AAABondDto;

            return request;
        }

        public IEnumerable<Type> SwitchCalculationExecutionRequest(CalculationParameterEncapsulator parameters)
        {
            List<Type> calculationExecutionTypes = new List<Type>();

            if (parameters.ExecuteGrahamCalculation)
                calculationExecutionTypes.Add(typeof(GrahamCalculationRequest));

            if (parameters.ExecuteDCFCalculation)
                calculationExecutionTypes.Add(typeof(DCFCalculationRequest));

            return calculationExecutionTypes;
        }
    }
}
