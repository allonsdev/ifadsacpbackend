using System.ComponentModel.DataAnnotations;

namespace sacpapi.Models
{
    public class UnitOfMeasurement
    {
        [Key]
        public int Id { get; set; }
        public required string  Name { get; set; }
        public string? CreatedBy { get; set; } = string.Empty;
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedDate { get; set; }
    }
}
