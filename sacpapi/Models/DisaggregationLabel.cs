namespace sacpapi.Models
{
    public class DisaggregationLabel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? UnitOfMeasurement { get; set; }
    }
}
