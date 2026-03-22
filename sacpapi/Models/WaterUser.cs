namespace sacpapi.Models
{
    using System;

    public class WaterUser
    {
        public int Id { get; set; } // PK usually not nullable

        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }

        public string? NameOfTheWaterPoint { get; set; }

        public string? FirstNameOfHhh { get; set; }
        public string? SurnameOfHhh { get; set; }

        public string? HouseholdIdentifier { get; set; }

        public string? Gender { get; set; }
        public int? Yob { get; set; }

        public string? AgeStatus { get; set; }

        public string? ContactNumber { get; set; }

        public string? GenderOfHhh { get; set; }

        public string? PwdStatus { get; set; }

        public int? FamilySize { get; set; }
        public int? Male { get; set; }
        public int? Female { get; set; }

        public int? NumberOfLivestockOwnedAccessingTheWaterPoint { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
