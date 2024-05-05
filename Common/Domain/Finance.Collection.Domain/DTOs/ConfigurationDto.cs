namespace Financial.Collection.Domain.DTOs
{
    public class ConfigurationDto
    {
        public Guid Id { get; set; }
        public decimal? SafetyMargin { get; set; }
        public decimal? DiscountRate { get; set; }
        public decimal? PerpetualValue { get; set; }
        //IntrinsicValueType
        public UserDto User { get; set; }
    }
}
