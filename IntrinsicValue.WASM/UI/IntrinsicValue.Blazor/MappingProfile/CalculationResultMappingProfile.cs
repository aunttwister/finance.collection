using AutoMapper;
using Finance.Collection.Domain.FinanceScraper.Results;
using FinanceScraper.Common.Init.Commands;
using IntrinsicValue.Blazor.Model;
using IntrinsicValue.Blazor.Services.FinanceServices.Encapsulation;
using IntrinsicValue.Calculation.DataSets.Results;
using IntrinsicValue.Calculation.Init.Commands;

namespace IntrinsicValue.Blazor.MappingProfile
{
    public class CalculationResultMappingProfile : Profile
    {
        public CalculationResultMappingProfile()
        {
            CreateMap<DCFCalculationResult, TickerDto>()
                .IncludeAllDerived()
                .ForMember(dest => dest.Ticker, opt => opt.MapFrom(src => src.Ticker))
                .ForMember(dest => dest.CurrentPrice, opt => opt.MapFrom(src => src));
        }
    }
}

