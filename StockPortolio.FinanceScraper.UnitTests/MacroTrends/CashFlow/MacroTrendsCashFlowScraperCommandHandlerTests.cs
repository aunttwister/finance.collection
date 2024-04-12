using AutoMapper;
using Moq;
using StockPortfolio.FinanceScraper.Common.Base;
using StockPortfolio.FinanceScraper.Common.Constants;
using StockPortfolio.FinanceScraper.Common.DataSets;
using StockPortfolio.FinanceScraper.MacroTrends.CashFlow.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPortolio.FinanceScraper.UnitTests.MacroTrends.CashFlow
{
    public class MacroTrendsCashFlowScraperCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldExecuteScrapeAndReturnResult()
        {
            // Arrange

            var ticker = "MSFT";

            var mockScrapeService = new Mock<IScrapeServiceStrategy<MacroTrendsCashOnHandScraperCommand, CashFlowDataSet>>();

            var handler = new MacroTrendsCashFlowScraperCommandHandler(mockScrapeService.Object);
            var request = new MacroTrendsCashFlowScraperCommand(ticker, UrlPathConstants.MacroTrendsCashFlowScraperPath);

            var expectedResult = new CashFlowDataSet(); // Replace with your expected result

            mockScrapeService
                .Setup(x => x.ExecuteScrape(request))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);

            // Additional assertions if needed
            // For example, you might want to verify that certain methods were called on the mock objects.
            // mockScrapeService.Verify(x => x.SomeMethod(), Times.Once);
        }
    }
}
