/*using AutoMapper;
using Finance.Collection.Domain.Common.Propagation;
using Finance.Collection.Domain.FinanceScraper.Results;
using Finance.Collection.Domain.IntrinsicValue.Calculation.Results;
using Financial.Collection.Domain.DTOs;
using Financial.Collection.Link.Blazor.WASM.Calculator.Services;
using Financial.Collection.Link.FinanceScraper.Encapsulation;
using Financial.Collection.Link.FinanceScraper.Services;
using Financial.Collection.Link.IntrinsicValue.Calculation.Encapsulator;
using Financial.Collection.Link.IntrinsicValue.Calculation.Services;
using IntrinsicValue.Calculation.Init.Commands;
using Moq;

namespace Financial.Collection.Link.Tests
{
    [TestFixture]
    public class ValuationAnalysisServiceTests
    {
        private Mock<IFinanceScraperService> _financeScraperServiceMock;
        private Mock<IValueCalculationService> _valueCalculationServiceMock;
        private Mock<IMapper> _mapperMock;
        private ValuationAnalysisService _service;

        [SetUp]
        public void Setup()
        {
            _financeScraperServiceMock = new Mock<IFinanceScraperService>();
            _valueCalculationServiceMock = new Mock<IValueCalculationService>();
            _mapperMock = new Mock<IMapper>();
            _service = new ValuationAnalysisService(_financeScraperServiceMock.Object,
                                                    _valueCalculationServiceMock.Object,
                                                    _mapperMock.Object);
        }

        [Test]
        public async Task PerformScrape_Success_ReturnsCorrectData()
        {
            // Arrange
            var scraperParam = new ScraperParameterEncapsulator();
            var scrapeResultMock = new MethodResult<IScrapeResult>
            {
                Data = this.new Mock<IScrapeResult>().Object,
                Exception = null
            };
            _financeScraperServiceMock.Setup(x => x.ScrapeFinancialDataAsync(It.IsAny<InitScrapeCommand>()))
                                      .ReturnsAsync(scrapeResultMock);
            _mapperMock.Setup(x => x.Map<TickerDto>(It.IsAny<IScrapeResult>()))
                       .Returns(new TickerDto());
            _mapperMock.Setup(x => x.Map<AAABondDto>(It.IsAny<IScrapeResult>()))
                       .Returns(new AAABondDto());

            // Act
            var result = await _service.PerformScrape(scraperParam);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Ticker, Is.Not.Null);
            Assert.That(result.AaaBond, Is.Not.Null);
            Assert.That(result.Exceptions, Is.Empty);
        }

        [Test]
        public async Task PerformScrape_Failure_CapturesExceptions()
        {
            // Arrange
            var scraperParam = new ScraperParameterEncapsulator();
            _financeScraperServiceMock.Setup(x => x.ScrapeFinancialDataAsync(It.IsAny<InitScrapeCommand>()))
                                      .ThrowsAsync(new InvalidOperationException("Scrape failed"));

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.PerformScrape(scraperParam));
        }

        [Test]
        public async Task PerformValuation_Success_ReturnsExpectedResult()
        {
            // Arrange
            var calcParam = new CalculationParameterEncapsulator { SafetyMargin = 150 }; // 1.5 as decimal
            var calcResultMock = new MethodResult<ICalculationResult>
            {
                Data = new Mock<ICalculationResult>().Object,
                Success = true
            };
            _valueCalculationServiceMock.Setup(x => x.CalculateFinancialDataAsync(It.IsAny<InitCalculationCommand>()))
                                        .ReturnsAsync(calcResultMock);
            _mapperMock.Setup(x => x.Map(It.IsAny<ICalculationResult>(), It.IsAny<TickerDto>()))
                       .Returns(new TickerDto());

            // Act
            var result = await _service.PerformValuation(calcParam);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Ticker, Is.Not.Null);
            Assert.That(result.Exceptions, Is.Empty);
        }


    }
}*/