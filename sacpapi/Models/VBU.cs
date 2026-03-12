using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sacpapi.Models
{
    [Table("VBUS")]
    public class VBUModel
    {
        [Column("HHD Identifier No")]
        public string? HhdIdentifierNo { get; set; }

        [Column("province")]
        public string? Province { get; set; }

        [Column("district")]
        public string? District { get; set; }

        [Column("Ward")]
        public double? Ward { get; set; }

        [Column("Village")]
        public string? Village { get; set; }

        [Column("Name")]
        public string? Name { get; set; }

        [Column("Surname")]
        public string? Surname { get; set; }

        [Column("Sex")]
        public string? Sex { get; set; }

        [Column("Age")]
        public double? Age { get; set; }

        [Column("Date of Birth")]
        public DateTime? DateOfBirth { get; set; }

        [Column("ID number")]
        public string? IdNumber { get; set; }

        [Column("Contact number")]
        public double? ContactNumber { get; set; }

        [Column("Marital status")]
        public string? MaritalStatus { get; set; }

        [Column("Do you have any form of disability?")]
        public string? Disability { get; set; }

        [Column("Sex of household head")]
        public string? HouseholdHeadSex { get; set; }

        [Column("VBU Land Size ")]
        public string? VbuLandSize { get; set; }

        [Column("Area under production")]
        public string? AreaUnderProduction { get; set; }

        [Column("Are you a member of any existing VBU")]
        public string? IsVbuMember { get; set; }

        [Column("what is the name of the VBU?")]
        public string? VbuName { get; set; }

        [Column("Do you have any leadership position in the group? ")]
        public string? LeadershipPosition { get; set; }

        [Column("Specify position")]
        public string? SpecifyPosition { get; set; }

        [Key]
        [Column("ID")]
        public int Id { get; set; }  // Usually primary keys are non-nullable
    }
}
