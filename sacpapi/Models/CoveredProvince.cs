using System.ComponentModel.DataAnnotations;

namespace sacpapi.Models
{
    public class CoveredProvince
    {
        [Key]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int ProvinceId { get; set; }

    }
}
