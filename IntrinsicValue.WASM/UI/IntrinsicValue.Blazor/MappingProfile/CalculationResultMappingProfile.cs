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
                .ForMember(dest => dest.Ticker, opt => opt.MapFrom(src => src.Ticker))
                .ForMember(dest => dest.SharesOutstanding, opt => opt.MapFrom(src => src.SharesOutstanding))
                .ForMember(dest => dest.TTMCashAndCashEquivalents, opt => opt.MapFrom(src => src.AssetsDataSet.TTMCashAndCashEquivalents))
                .ForMember(dest => dest.TTMTotalDebt, opt => opt.MapFrom(src => src.LiabilitiesDataSet.TTMTotalDebt))
                .ForMember(dest => dest.CurrentPrice, opt => opt.MapFrom(src => src.CurrentPrice))
                .ForMember(dest => dest.DCFModel, opt => opt.MapFrom(src => new DCFModelDto
                {
                    Value = src.ValuationDataSet.DiscountedCashFlowValue.Value,
                    PriceDifference = src.PriceDifference,
                    PriceDifferencePercentage = src.PriceDifferencePercent
                }))
                .ForMember(dest => dest.GrowthRate, opt => opt.MapFrom(src => new GrowthRateDto
                {
                    Period = src.GrowthRateDataSet.Period,
                    Rate = src.GrowthRateDataSet.AverageGrowthRate
                }));
        }
    }
}

