using System.ComponentModel.DataAnnotations;

namespace sacpapi.Models
{
    public class MenuItem
    {
        [Key]
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public required string Name { get; set; }
        public string? Url { get; set; }
        public required string uid { get; set; }
        public bool ShowMenu { get; set; } = false;
        public int Index { get; set; } = 0;
    }
}
