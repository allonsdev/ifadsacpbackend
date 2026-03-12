using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sacpapi.Models
{
    [Table("SubActivity")]
    public class SubActivity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? SubActivityId { get; set; }  // nullable in SQL

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string ActivityId { get; set; } = string.Empty;
    }
}
