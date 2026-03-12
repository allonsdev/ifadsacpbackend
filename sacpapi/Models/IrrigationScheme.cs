using System.ComponentModel.DataAnnotations;

namespace sacpapi.Models
{
    public class IrrigationScheme
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Province { get; set; }
        public string? AgroEcologicalRegion { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public string? SchemeManagementModel { get; set; }
        public DateTime DateEstablished { get; set; }
        public double? TotalDevelopedAreaToDate { get; set; }
        public double? AreaUnderIrrigation { get; set; }
        public double? PotentialAreaOfScheme { get; set; }
        public string? IrrigationSchemeStatus { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public int? NumberOfIndividuals { get; set; }
        public int? Women { get; set; }
        public int? Men { get; set; }
        public int? Youth { get; set; }
    }
}
