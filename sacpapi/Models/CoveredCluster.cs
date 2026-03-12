using System.ComponentModel.DataAnnotations;

namespace sacpapi.Models
{
    public class CoveredCluster
    {
        [Key]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int ClusterId { get; set; }
    }
}
