using System.ComponentModel.DataAnnotations;

namespace sacpapi.Models
{
    public class District
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProvinceId { get; set; }
    }
}
