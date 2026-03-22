namespace sacpapi.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class SchoolBusinessUnit
    {
        public int Id { get; set; }

        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }

        public string? NameOfSchoolBusinessUnit { get; set; }

        public int? NumberOfFemaleStudents { get; set; }
        public int? NumberOfMaleStudents { get; set; }

        // Computed column (SQL handles this)
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? TotalNumberOfStudents { get; private set; }

        public string? NameOfSchoolHead { get; set; }
        public string? ContactNumberOfTheSchoolHead { get; set; }

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
