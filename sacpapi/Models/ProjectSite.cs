using System.ComponentModel.DataAnnotations;

namespace sacpapi.Models
{
    public class ProjectSite
    {
        [Key] 
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string SiteType { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Province { get; set; }
        public string District { get; set; }

    }
}
