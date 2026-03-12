using System.ComponentModel.DataAnnotations;

namespace sacpapi.Models
{
    public class CoveredWard
    {
        [Key]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int WardCode { get; set; }
    }
}
