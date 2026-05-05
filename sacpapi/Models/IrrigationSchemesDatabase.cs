namespace sacpapi.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tblrrigationSchemesDatabase")]
    public class IrrigationSchemesDatabase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(100)]
        public string? Province { get; set; }

        [StringLength(100)]
        public string? District { get; set; }

        [StringLength(50)]
        public string? Ward { get; set; }

        [StringLength(150)]
        public string? IrrigationScheme { get; set; }

        [Required]
        [StringLength(100)]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "FirstName must not contain numbers")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Surname must not contain numbers")]
        public string Surname { get; set; }

        [StringLength(100)]
        public string? HouseholdIdentifier { get; set; }

        [RegularExpression("Male|Female", ErrorMessage = "Gender must be Male or Female")]
        public string? GenderOfRegisteredPerson { get; set; }

        public int? YearOfBirth { get; set; }

        [StringLength(50)]
        public string? YouthStatus { get; set; }

        [StringLength(50)]
        public string? MaritalStatus { get; set; }

        [StringLength(20)]
        public string? ContactNumber { get; set; }

        [RegularExpression("Male|Female", ErrorMessage = "Gender must be Male or Female")]
        public string? GenderOfHouseholdHead { get; set; }

        [StringLength(20)]
        public string? PwD { get; set; }

        public int? FamilySize { get; set; }

        public int? Male { get; set; }

        public int? Female { get; set; }

        [StringLength(100)]
        public string? ImcPosition { get; set; }

        [StringLength(100)]
        public string? LeadershipPosition { get; set; }

        [StringLength(100)]
        public string? NameOfChairperson { get; set; }

        [StringLength(20)]
        public string? ContactNumberOfChairperson { get; set; }
    }
}