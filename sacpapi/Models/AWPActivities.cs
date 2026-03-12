using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace sacpapi.Models
{
    [Table("AWPActivities")]
    public class AWPActivity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string SubComponentId { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;
    }
}
