using System.ComponentModel.DataAnnotations;

namespace sacpapi.Models
{
    public class CoveredDistrict
    {
        [Key]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int DistrictId { get; set; }
    }
}
