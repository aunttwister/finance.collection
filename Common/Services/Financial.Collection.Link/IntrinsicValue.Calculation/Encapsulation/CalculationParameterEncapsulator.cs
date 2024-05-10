using Financial.Collection.Domain.DTOs;

namespace Financial.Collection.Link.IntrinsicValue.Calculation.Encapsulator
{
    public class CalculationParameterEncapsulator
    {
        public TickerDto TickerDto { get; set; }
        public AAABondDto AAABondDto { get; set; }
        public bool ExecuteGrahamCalculation { get; set; }
        public bool ExecuteDCFCalculation { get; set; }
        public decimal SafetyMargin { get; set; } = 1;
    }
}
