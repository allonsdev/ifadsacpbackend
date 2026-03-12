namespace sacpapi.Models
{
    public class Outcome
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int ObjectiveId { get; set; } = 0;
    }
}
