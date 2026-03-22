namespace sacpapi.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class EmploymentRecord
    {
        public int Id { get; set; }

        public string? District { get; set; }
        public string? Gender { get; set; }

        public string? AgeGroup { get; set; }
        public string? DisabilityStatus { get; set; }

        public int? PwdMale { get; set; }
        public int? PwdFemale { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? Total { get; private set; }

        public DateTime? CreatedAt { get; set; }
    }
}
