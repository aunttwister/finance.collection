using Financial.Collection.Domain.DTOs;

namespace Financial.Collection.Link.IntrinsicValue.Calculation.Encapsulator
{
    public class CalculationParameterEncapsulator
    {
        public IEnumerable<Type> ScrapeTypes { get; set; }
        public TickerDto TickerDto { get; set; }
        public AAABondDto AAABondDto { get; set; }
        public decimal SafetyMargin { get; set; } = 1;
    }
}
