using System.ComponentModel.DataAnnotations;

namespace sacpapi.Models
{
    public class ClusterWard
    {
        [Key]
        public int Id { get; set; }
        public int ClusterId { get; set; }
        public int WardCode { get; set; }
    }
}
