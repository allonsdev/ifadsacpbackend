using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sacpapi.Models
{
    [Table("ActivityMapping")]
    public class ActivityMapping
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string ComponentId { get; set; } = string.Empty;

        [Required]
        public string SubComponentId { get; set; } = string.Empty;

        [Required]
        public string ActivityId { get; set; } = string.Empty;

        [Required]
        public string SubActivityId { get; set; } = string.Empty;
    }
}
