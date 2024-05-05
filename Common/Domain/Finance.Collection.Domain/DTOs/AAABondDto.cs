namespace Financial.Collection.Domain.DTOs
{
    public class AAABondDto
    {
        public Guid Id { get; set; }
        public decimal  CurrentYield { get; set; }
        public decimal AverageYield { get; set; }
        public DateTime Date { get; set; }
    }
}
