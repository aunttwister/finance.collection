using AutoMapper;
using Finance.Collection.Domain.FinanceScraper.Results;
using Finance.Collection.Domain.IntrinsicValue.Calculation.Results;
using FinanceScraper.Common.Init.Commands;
using IntrinsicValue.Blazor.Model;
using IntrinsicValue.Blazor.Services.FinanceServices.Encapsulation;
using IntrinsicValue.Calculation.DataSets.Results;
using IntrinsicValue.Calculation.Init.Commands;

namespace IntrinsicValue.Blazor.MappingProfile
{
    public class FinancialAnalysisMappingProfile : Profile
    {
        public FinancialAnalysisMappingProfile()
        {
            CreateMap<CalculatorParameterEncapsulator, InitScrapeCommand>()
                .ForMember(dest => dest.Ticker, opt => opt.MapFrom(src => src.Ticker))
                .ForMember(dest => dest.ExecuteDCFScrape, opt => opt.MapFrom(src => src.ExecuteDCFScrape))
                .ForMember(dest => dest.ExecuteGrahamScrape, opt => opt.MapFrom(src => src.ExecuteGrahamScrape))
                .ForMember(dest => dest.UseHtmlContent, opt => opt.MapFrom(src => src.UseHtmlContent));
            CreateMap<IScrapeResult, InitCalculationCommand>()
                .IncludeAllDerived()
                .ForMember(dest => dest.Ticker, opt => opt.MapFrom(src => src.Ticker))
                .ForMember(dest => dest.ScrapeResult, opt => opt.MapFrom(src => src))
                .AfterMap((src, dest, context) =>
                {
                    if (context.Items["safetyMargin"] is decimal safetyMargin)
                    {
                        dest.SafetyMargin = safetyMargin / 100;
                    }
                });
        }
    }
}
