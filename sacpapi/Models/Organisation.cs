using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sacpapi.Models
{
    public class Organisation
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Type { get; set; } = 0;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int DistrictId { get; set; } = 0;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Latitude { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Longitude { get; set; } = 0;
        public string? ContactName { get; set; }
        public int? ContactNo { get; set; }
        public string? EmailAddress { get; set; }
        public string? CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string? UpdatedBy { get; set; } = string.Empty;
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}