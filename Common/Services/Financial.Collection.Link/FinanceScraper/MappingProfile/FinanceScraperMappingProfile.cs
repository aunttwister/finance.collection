﻿using AutoMapper;
using Finance.Collection.Domain.Common.Helpers.AutoMapper;
using Finance.Collection.Domain.FinanceScraper.Results;
using Finance.Collection.Domain.IntrinsicValue.Calculation.Results;
using FinanceScraper.Common.Init.Commands;
using Financial.Collection.Domain.DTOs;
using Financial.Collection.Link.FinanceScraper.Encapsulation;
using IntrinsicValue.Calculation.DataSets.Results;
using IntrinsicValue.Calculation.Init.Commands;

namespace Financial.Collection.Link.FinanceScraper.MappingProfile
{
    public class FinanceScraperMappingProfile : Profile
    {
        public FinanceScraperMappingProfile()
        {
            CreateMap<ScraperParameterEncapsulator, InitScrapeCommand>()
                .ForMember(dest => dest.Ticker, opt => opt.MapFrom(src => src.Ticker))
                .ForMember(dest => dest.ExecuteDCFScrape, opt => opt.MapFrom(src => src.ExecuteDCFScrape))
                .ForMember(dest => dest.ExecuteGrahamScrape, opt => opt.MapFrom(src => src.ExecuteGrahamScrape))
                .ForMember(dest => dest.UseHtmlContent, opt => opt.MapFrom(src => src.UseHtmlContent));

            CreateMap<IScrapeResult, TickerDto>()
                .Include<CurrentPriceScrapeResult, TickerDto>()
                .Include<DCFScrapeResult, TickerDto>()
                .Include<GrahamScrapeResult, TickerDto>()
                .Include<CombinedScrapeResult, TickerDto>();

            CreateMap<CurrentPriceScrapeResult, TickerDto>()
                .ForMember(dest => dest.Symbol, opt => opt.MapFrom(src => src.Ticker))
                .ForMember(dest => dest.CurrentPrice, opt => opt.MapFrom(src => src.CurrentPrice.CurrentPrice.Data))
                .ForMember(dest => dest.ScrapeDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<GrahamScrapeResult, TickerDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Symbol, opt => 
                    opt.MapFrom(src => src.Ticker))
                .ForMember(dest => dest.CurrentPrice, opt => opt.Ignore())
                .ForMember(dest => dest.EPS, opt => 
                    opt.MapFrom(src => src.Summary.Eps.Data))
                .ForMember(dest => dest.PE, opt => opt.Ignore())
                .ForMember(dest => dest.ExpectedFiveYearGrowth, opt => 
                    opt.MapFrom(src => src.Analysis.FiveYearGrowth.Data))
                .ForMember(dest => dest.SharesOutstanding, opt => opt.Ignore())
                .ForMember(dest => dest.TTMCashAndCashEquivalents, opt => opt.Ignore())
                .ForMember(dest => dest.TTMTotalDebt, opt => opt.Ignore())
                .ForMember(dest => dest.Notes, opt => opt.Ignore())
                .ForMember(dest => dest.EarningsDate, opt => opt.Ignore())
                .ForMember(dest => dest.Tickerlists, opt => opt.Ignore())
                .ForMember(dest => dest.TickerIntrinsicValues, opt => opt.Ignore())
                .ForSourceMember(src => src.TripleABonds, opt => opt.DoNotValidate())
                .ForMember(dest => dest.ScrapeDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<DCFScrapeResult, TickerDto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Symbol, opt => 
                    opt.MapFrom(src => src.Ticker))
                .ForMember(dest => dest.CurrentPrice, opt => opt.Ignore())
                .ForMember(dest => dest.EPS, opt => opt.Ignore())
                .ForMember(dest => dest.PE, opt => opt.Ignore())
                .ForMember(dest => dest.SharesOutstanding, opt => 
                    opt.MapFrom(src => src.Statistics.SharesOutstanding.Data))
                .ForMember(dest => dest.TTMCashAndCashEquivalents, opt => 
                    opt.MapFrom(src => src.BalanceSheet.TTMCashEquivalents.Data))
                .ForMember(dest => dest.TTMTotalDebt, opt => 
                    opt.MapFrom(src => src.BalanceSheet.TTMTotalDebt.Data))
                .ForMember(dest => dest.Notes, opt => opt.Ignore())
                .ForMember(dest => dest.EarningsDate, opt => opt.Ignore())
                .ForMember(dest => dest.YearlyData, opt => opt.Ignore())
                    .AfterMap((src, dest) =>
                    {
                        dest.YearlyData = GenerateYearlyData(src);
                    })
                .ForMember(dest => dest.Tickerlists, opt => opt.Ignore())
                .ForMember(dest => dest.TickerIntrinsicValues, opt => opt.Ignore())
                .ForMember(dest => dest.ScrapeDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<CombinedScrapeResult, TickerDto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Symbol, opt => opt.MapFrom(src => src.Ticker))
                .ForMember(dest => dest.CurrentPrice, opt => opt.MapFrom(src => MappingHelpers.MapCurrentPrice(src)))
                .ForMember(dest => dest.EPS, opt => opt.MapFrom(src => MappingHelpers.MapEPS(src)))
                .ForMember(dest => dest.PE, opt => opt.Ignore())
                .ForMember(dest => dest.ExpectedFiveYearGrowth, opt => opt.MapFrom(src => MappingHelpers.MapExpectedFiveYearGrowth(src)))
                .ForMember(dest => dest.SharesOutstanding, opt => opt.MapFrom(src => MappingHelpers.MapSharesOutstanding(src)))
                .ForMember(dest => dest.TTMCashAndCashEquivalents, opt => opt.MapFrom(src => MappingHelpers.MapTTMCashAndCashEquivalents(src)))
                .ForMember(dest => dest.TTMTotalDebt, opt => opt.MapFrom(src => MappingHelpers.MapTTMTotalDebt(src)))
                .ForMember(dest => dest.Notes, opt => opt.Ignore())
                .ForMember(dest => dest.EarningsDate, opt => opt.Ignore())
                .ForMember(dest => dest.YearlyData, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    var dcfs = src.GetResult<DCFScrapeResult>();
                    if (dcfs != null)
                    {
                        dest.YearlyData = GenerateYearlyData(dcfs);
                    }
                })
                .ForMember(dest => dest.Tickerlists, opt => opt.Ignore())
                .ForMember(dest => dest.TickerIntrinsicValues, opt => opt.Ignore())
                .ForMember(dest => dest.ScrapeDate, opt => opt.MapFrom(src => DateTime.UtcNow));



            //AAABondDto mappings
            CreateMap<GrahamScrapeResult, AAABondDto>()
               .ForMember(dest => dest.Id, opt => opt.Ignore())
               .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.UtcNow))
               .ForMember(dest => dest.CurrentYield, opt =>
                     opt.MapFrom(src => src.TripleABonds.CurrentTripleABond.IsSuccessful ?
                        src.TripleABonds.CurrentTripleABond.Data : 0m))
               .ForMember(dest => dest.AverageYield, opt =>
                     opt.MapFrom(src => src.TripleABonds.HistoricalAverageTripleABond.IsSuccessful ?
                        src.TripleABonds.HistoricalAverageTripleABond.Data : 0m));

            CreateMap<CombinedScrapeResult, AAABondDto>()
               .ForMember(dest => dest.Id, opt => opt.Ignore())
               .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.UtcNow))
               .ForMember(dest => dest.CurrentYield, opt =>
                     opt.MapFrom(src => src.GetResult<GrahamScrapeResult>().TripleABonds.CurrentTripleABond.IsSuccessful ?
                        src.GetResult<GrahamScrapeResult>().TripleABonds.CurrentTripleABond.Data : 0m))
               .ForMember(dest => dest.AverageYield, opt =>
                     opt.MapFrom(src => src.GetResult<GrahamScrapeResult>().TripleABonds.HistoricalAverageTripleABond.IsSuccessful ?
                        src.GetResult<GrahamScrapeResult>().TripleABonds.HistoricalAverageTripleABond.Data : 0m));

        }

        private List<YearlyDataDto> GenerateYearlyData(DCFScrapeResult src)
        {
            var yearlyData = new Dictionary<string, YearlyDataDto>();

            if (src.CashFlow.HistoricalCashFlows.IsSuccessfulKey && src.CashFlow.HistoricalCashFlows.IsSuccessfulValue)
            {
                // Merge data from HistoricalCashFlow
                foreach (var entry in src.CashFlow.HistoricalCashFlows.Data)
                {
                    if (!yearlyData.ContainsKey(entry.Key))
                    {
                        yearlyData[entry.Key] = new YearlyDataDto { Id = Guid.NewGuid(), Year = entry.Key };
                    }
                    yearlyData[entry.Key].CashFlow = entry.Value;
                }
            }

            if (src.BalanceSheet.HistoricalTotalDebt.IsSuccessfulKey && src.BalanceSheet.HistoricalTotalDebt.IsSuccessfulValue)
            {
                // Merge data from HistoricalTotalDebt
                foreach (var entry in src.BalanceSheet.HistoricalTotalDebt.Data)
                {
                    if (!yearlyData.ContainsKey(entry.Key))
                    {
                        yearlyData[entry.Key] = new YearlyDataDto { Id = Guid.NewGuid(), Year = entry.Key };
                    }
                    yearlyData[entry.Key].TotalDebt = entry.Value;
                }
            }


            if (src.BalanceSheet.HistoricalCashEquivalents.IsSuccessfulKey && src.BalanceSheet.HistoricalCashEquivalents.IsSuccessfulValue)
            {
                // Merge data from HistoricalCashAndCashEquivalents
                foreach (var entry in src.BalanceSheet.HistoricalCashEquivalents.Data)
                {
                    if (!yearlyData.ContainsKey(entry.Key))
                    {
                        yearlyData[entry.Key] = new YearlyDataDto { Id = Guid.NewGuid(), Year = entry.Key };
                    }
                    yearlyData[entry.Key].CashAndCashEquivalents = entry.Value;
                }
            }
                

            // Convert dictionary to list
            return yearlyData.Values.ToList();
        }
    }
}
