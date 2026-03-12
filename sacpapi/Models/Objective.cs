namespace sacpapi.Models
{
    public class Objective
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int ProjectId { get; set; } = 0;
    }
}
