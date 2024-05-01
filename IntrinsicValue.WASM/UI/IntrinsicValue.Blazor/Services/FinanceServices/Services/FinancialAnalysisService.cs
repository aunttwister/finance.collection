using AutoMapper;
using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.FinanceScraper.Results;
using Finance.Collection.Domain.IntrinsicValue.Calculation.Results;
using FinanceScraper.Common.Init.Commands;
using IntrinsicValue.Blazor.Services.FinanceServices.Encapsulation;
using IntrinsicValue.Blazor.Services.FinanceServices.Interfaces;
using IntrinsicValue.Calculation.Init.Commands;
using System.Reflection;

namespace IntrinsicValue.Blazor.Services.FinanceServices.Services
{
    public class FinancialAnalysisService : IFinancialAnalysisService
    {
        private readonly IFinanceScraperService _financeScraperService;
        private readonly IValueCalculationService _valueCalculationService;
        private readonly IMapper _mapper;

        public FinancialAnalysisService(
            IFinanceScraperService financeScraperService,
            IValueCalculationService valueCalculationService,
            IMapper mapper)
        {
            _financeScraperService = financeScraperService;
            _valueCalculationService = valueCalculationService;
            _mapper = mapper;
        }

        public async Task<MethodResult<ICalculationResult>> PerformAnalysis(CalculatorParameterEncapsulator parameters)
        {
            InitScrapeCommand requestScrape = CreateInitScrapeCommand(parameters);

            MethodResult<IScrapeResult> scrapeResult = await _financeScraperService.ScrapeFinancialDataAsync(requestScrape);

            InitCalculationCommand requestCalc = PrepareCalculationCommand(scrapeResult.Data, parameters.SafetyMargin);

            return await _valueCalculationService.CalculateFinancialDataAsync(requestCalc);
        }

        public InitScrapeCommand CreateInitScrapeCommand(CalculatorParameterEncapsulator encapsulator)
        {
            return _mapper.Map<InitScrapeCommand>(encapsulator);
        }

        public InitCalculationCommand PrepareCalculationCommand(IScrapeResult result, decimal safetyMargin)
        {
            var context = new MapperConfiguration(cfg => {
                cfg.AddMaps(Assembly.GetExecutingAssembly());
            }).CreateMapper();
            return context.Map<IScrapeResult, InitCalculationCommand>(result, opts =>
            {
                opts.Items["safetyMargin"] = safetyMargin;
            });
        }
    }
}
