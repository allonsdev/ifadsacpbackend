namespace sacpapi.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EmploymentCreation")]
    public class EmploymentRecord
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string? Province { get; set; }

        [StringLength(100)]
        public string? District { get; set; }

        [StringLength(50)]
        public string? Ward { get; set; }

        [StringLength(100)]
        public string? NameOfVillage { get; set; }

        [Required]
        [StringLength(100)]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "FirstName must not contain numbers")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Surname must not contain numbers")]
        public string Surname { get; set; }

        [Required]
        [StringLength(13, MinimumLength = 13)]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "National ID must be 13 digits")]
        public string NationalIdNumber { get; set; }

        [Required]
        [RegularExpression("Male|Female", ErrorMessage = "Gender must be Male or Female")]
        public string Gender { get; set; }

        public int? Yob { get; set; }

        [StringLength(50)]
        public string? YouthStatus { get; set; }

        [StringLength(50)]
        public string? MaritalStatus { get; set; }

        [StringLength(20)]
        public string? ContactNumber { get; set; }

        [RegularExpression("Male|Female")]
        public string? GenderOfHhh { get; set; }

        [StringLength(50)]
        public string? PwDStatus { get; set; }

        public int? FamilySize { get; set; }
        public int? Male { get; set; }
        public int? Female { get; set; }

        [StringLength(100)]
        public string? Employer { get; set; }

        [StringLength(100)]
        public string? TypeOfEmployment { get; set; }

        [StringLength(100)]
        public string? DurationOfEmploymentWorkContract { get; set; }

        [StringLength(200)]
        public string? MainServiceRendered { get; set; }

        [StringLength(100)]
        public string? Designation { get; set; }

        public decimal? AnnualIncomeUsd { get; set; }

        [StringLength(10)]
        public string? HaveYouReceivedAnyTrainingFromSacp { get; set; }

        [StringLength(255)]
        public string? SpecifyTrainingMostBeneficial { get; set; }

        // ✅ Computed Total (Male + Female)
        [NotMapped]
        public int? Total => (Male ?? 0) + (Female ?? 0);

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}