using System.ComponentModel.DataAnnotations;

namespace sacpapi.Models
{
    public class Functionality
    {
        [Key] 
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}
