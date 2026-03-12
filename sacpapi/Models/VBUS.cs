using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace sacpapi.Models
{
    [Table("VBUS")]
    public class VBUS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }  // Identity column (non-nullable)

        [Column("HHD Identifier No")]
        public string? HHDIdentifierNo { get; set; }

        public string? Province { get; set; }
        public string? District { get; set; }

        public double? Ward { get; set; } // FLOAT in SQL corresponds to double in C#

        public string? Village { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Sex { get; set; }

        public double? Age { get; set; } // FLOAT in SQL corresponds to double in C#

        [Column("Date of Birth")]
        public DateTime? DateOfBirth { get; set; }

        [Column("ID number")]
        public string? IDNumber { get; set; }

        [Column("Contact number")]
        public double? ContactNumber { get; set; } // FLOAT in SQL corresponds to double in C#

        [Column("Marital status")]
        public string? MaritalStatus { get; set; }

        [Column("Do you have any form of disability?")]
        public string? DisabilityStatus { get; set; }

        [Column("Sex of household head")]
        public string? HouseholdHeadSex { get; set; }

        [Column("VBU Land Size ")]
        public string? VBULandSize { get; set; }

        [Column("Area under production")]
        public string? AreaUnderProduction { get; set; }

        [Column("Are you a member of any existing VBU")]
        public string? MemberOfVBU { get; set; }

        [Column("what is the name of the VBU?")]
        public string? VBUName { get; set; }

        [Column("Do you have any leadership position in the group?")]
        public string? LeadershipPosition { get; set; }

        [Column("Specify position")]
        public string? Position { get; set; }
    }
}
