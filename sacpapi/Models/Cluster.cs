using System.ComponentModel.DataAnnotations;

namespace sacpapi.Models
{
    public class Cluster
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
