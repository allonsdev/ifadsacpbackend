using System.ComponentModel.DataAnnotations;

namespace sacpapi.Models
{
    public class ActivitySubComponent
    {
        [Key]
        public int Id { get; set; }
        public int SubComponentId { get; set; }
        public int ActivityId { get; set; }
    }
}
