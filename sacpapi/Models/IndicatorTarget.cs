using System.ComponentModel.DataAnnotations;

namespace sacpapi.Models
{
    public class IndicatorTarget
    {
        [Key]
        public int Id { get; set; } = 0;
        public int IndicatorId { get; set; }
        public string Organization { get; set; }
        public string District { get; set; }
        public int FinancialYear { get; set; }
        public string Quarter { get; set; }
        public string Month { get; set; }
        public int Target { get; set; }
        public int? Achievement { get; set; }
        public string Comments { get; set; }
    }
}
