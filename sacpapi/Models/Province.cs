using System.ComponentModel.DataAnnotations;

namespace sacpapi.Models
{
    public class Province
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
    }
}
