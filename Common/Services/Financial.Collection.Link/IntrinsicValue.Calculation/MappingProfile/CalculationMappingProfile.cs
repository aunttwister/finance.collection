using AutoMapper;
using Finance.Collection.Domain.FinanceScraper.Results;
using Finance.Collection.Domain.IntrinsicValue.Calculation.Results;
using Finance.Collection.Domain.IntrinsicValue.Calculation.Results.DCFIntrinsicModel;
using FinanceScraper.Common.Init.Commands;
using Financial.Collection.Domain.DTOs;
using IntrinsicValue.Calculation.DataSets.Results;
using IntrinsicValue.Calculation.Init.Commands;

namespace Financial.Collection.Link.IntrinsicValue.Calculation.MappingProfile
{
    public class CalculationMappingProfile : Profile
    {
        public CalculationMappingProfile()
        {
            CreateMap<TickerDto, InitCalculationCommand>()
                .ForMember(dest => dest.TickerDto, opt => opt.MapFrom(src => src));

            CreateMap<AAABondDto, InitCalculationCommand>()
                .ForMember(dest => dest.AAABondDto, opt => opt.MapFrom(src => src));

            //ICalculationResult -> TickerDto

            CreateMap<ICalculationResult, TickerDto>()
                .Include<CombinedCalculationResult, TickerDto>()
                .Include<GrahamCalculationResult, TickerDto>()
                .Include<DCFCalculationResult, TickerDto>();

            CreateMap<GrahamCalculationResult, TickerDto>()
                .ForMember(dest => dest.TickerIntrinsicValues, opt => opt.Ignore())
                    .AfterMap((src, dest) =>
                    {
                        dest.TickerIntrinsicValues.Add(new HistoricalTickerIntrinsicValueDto()
                        {
                            IntrinsicValueType = "GrahamModel",
                            IntrinsicValue = src.IntrinsicValue,
                            BuyPrice = src.BuyPrice,
                            PriceDifference = src.PriceDifference,
                            PriceDifferencePercentage = src.PriceDifferencePercent,
                            DateTime = DateTime.UtcNow,
                            SafetyMargin = src.SafetyMargin
                        });
                    });

            CreateMap<DCFCalculationResult, TickerDto>()
                .ForMember(dest => dest.TickerIntrinsicValues, opt => opt.Ignore())
                    .AfterMap((src, dest) =>
                    {
                        dest.TickerIntrinsicValues.Add(new HistoricalTickerIntrinsicValueDto()
                        {
                            IntrinsicValueType = "DCFModel",
                            IntrinsicValue = src.IntrinsicValue,
                            BuyPrice = src.BuyPrice,
                            PriceDifference = src.PriceDifference,
                            PriceDifferencePercentage = src.PriceDifferencePercent,
                            DateTime = DateTime.UtcNow,
                            SafetyMargin = src.SafetyMargin
                        });
                    })
                .ForMember(dest => dest.YearlyData, opt => opt.Ignore())
                    .AfterMap((src, dest) =>
                    {
                        MergeEstimatedCashFlow(dest.YearlyData, src.EstimatedCashFlows);
                    })
                .ForMember(dest => dest.YearlyData, opt => opt.Ignore())
                    .AfterMap((src, dest) =>
                    {
                        MergeHistoricalGrowthRate(dest.YearlyData, src.HistoricalGrowthRate.HistoricalGrowthRates);
                    })
                .ForMember(dest => dest.AverageGrowthRatePeriod, opt => opt.MapFrom(src => src.HistoricalGrowthRate.AveragePeriod))
                .ForMember(dest => dest.AverageGrowthRate, opt => opt.MapFrom(src => src.HistoricalGrowthRate.AverageGrowthRate));

            CreateMap<CombinedCalculationResult, TickerDto>()
                .ForMember(dest => dest.TickerIntrinsicValues, opt => opt.Ignore())
                    .AfterMap((src, dest) =>
                    {
                        if (src.GetResult<GrahamCalculationResult>() != null)
                        {
                            GrahamCalculationResult grahamCalculationResult = src.GetResult<GrahamCalculationResult>();
                            dest.TickerIntrinsicValues.Add(new HistoricalTickerIntrinsicValueDto()
                            {
                                IntrinsicValueType = "GrahamModel",
                                IntrinsicValue = grahamCalculationResult.IntrinsicValue,
                                BuyPrice = grahamCalculationResult.BuyPrice,
                                PriceDifference = grahamCalculationResult.PriceDifference,
                                PriceDifferencePercentage = grahamCalculationResult.PriceDifferencePercent,
                                DateTime = DateTime.UtcNow,
                                SafetyMargin = grahamCalculationResult.SafetyMargin
                            });
                        }

                        if (src.GetResult<DCFCalculationResult>() != null)
                        {
                            DCFCalculationResult dcfCalculationResult = src.GetResult<DCFCalculationResult>();
                            dest.TickerIntrinsicValues.Add(new HistoricalTickerIntrinsicValueDto()
                            {
                                IntrinsicValueType = "DCFModel",
                                IntrinsicValue = dcfCalculationResult.IntrinsicValue,
                                BuyPrice = dcfCalculationResult.BuyPrice,
                                PriceDifference = dcfCalculationResult.PriceDifference,
                                PriceDifferencePercentage = dcfCalculationResult.PriceDifferencePercent,
                                DateTime = DateTime.UtcNow,
                                SafetyMargin = dcfCalculationResult.SafetyMargin
                            });
                        }
                    })
                .ForMember(dest => dest.YearlyData, opt => opt.Ignore())
                    .AfterMap((src, dest) =>
                    {
                        MergeEstimatedCashFlow(dest.YearlyData, src.GetResult<DCFCalculationResult>().EstimatedCashFlows);
                    })
                .ForMember(dest => dest.YearlyData, opt => opt.Ignore())
                    .AfterMap((src, dest) =>
                    {
                        MergeHistoricalGrowthRate(dest.YearlyData, src.GetResult<DCFCalculationResult>().HistoricalGrowthRate.HistoricalGrowthRates);
                    })
                .ForMember(dest => dest.AverageGrowthRatePeriod, opt => opt.MapFrom(src => src.GetResult<DCFCalculationResult>().HistoricalGrowthRate.AveragePeriod))
                .ForMember(dest => dest.AverageGrowthRate, opt => opt.MapFrom(src => src.GetResult<DCFCalculationResult>().HistoricalGrowthRate.AverageGrowthRate)); ;
        }

        private static void MergeEstimatedCashFlow(List<YearlyDataDto> yearlyDataList, IEnumerable<EstimatedCashFlowResultDataSet> estimatedDataList)
        {
            // Create a dictionary from the estimated data list
            var estimatedDataDict = estimatedDataList.ToDictionary(x => x.Year, x => x);

            // Iterate through each YearlyDataDto and update the relevant properties
            foreach (var estimatedEntry in estimatedDataDict)
            {
                bool keyFound = false;
                foreach (var yearlyData in yearlyDataList)
                {
                    if (yearlyData.Year.Equals(estimatedEntry.Key))
                    {
                        yearlyData.EstimatedCashFlow = estimatedEntry.Value.EstimatedCashFlow;
                        yearlyData.EstimatedPresentValue = estimatedEntry.Value.EstimatedPresentValue;
                        keyFound = true;
                    }
                    else
                    {
                        continue;
                    }
                }
                if (!keyFound)
                {
                    yearlyDataList.Add(new YearlyDataDto()
                    {
                        Year = estimatedEntry.Key,
                        EstimatedCashFlow = estimatedEntry.Value.EstimatedCashFlow,
                        EstimatedPresentValue = estimatedEntry.Value.EstimatedPresentValue
                    });
                }
            }
        }

        private static void MergeHistoricalGrowthRate(List<YearlyDataDto> yearlyDataList, Dictionary<string, decimal> historicalGrowthRates)
        {
            // Iterate through each YearlyDataDto and update the relevant properties
            foreach (var yearlyData in yearlyDataList)
            {
                if (historicalGrowthRates.TryGetValue(yearlyData.Year, out var historicalGrowthRate))
                {
                    yearlyData.GrowthRate = historicalGrowthRate;
                }
            }
        }
    }
}