using System.ComponentModel.DataAnnotations;

namespace sacpapi.Models
{
    public class WaterPoint
    {
        [Key] 
        public int Id { get; set; }
        public string? Name { get; set;}
        public string? Province { get; set; }
        public string?  District { get; set; }
        public string? Ward { get; set; }
        public string? Village { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? NumberOfHouseholds { get; set; }
        public int? NumberOfIndividuals { get; set; }
        public int? Men { get; set; }
        public int? Women { get; set; }
        public int? Youth { get; set; }
    }
}
