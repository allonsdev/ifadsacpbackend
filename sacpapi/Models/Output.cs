namespace sacpapi.Models
{
    public class Output
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int OutcomeId { get; set; } = 0;
    }
}
