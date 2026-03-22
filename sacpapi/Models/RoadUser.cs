namespace sacpapi.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class RoadUser
    {
        public int Id { get; set; }

        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }

        public string? FirstName { get; set; }
        public string? Surname { get; set; }

        public string? NationalIdNumber { get; set; }

        public string? GenderOfHhh { get; set; }
        public int? Yob { get; set; }

        public string? YouthStatus { get; set; }
        public string? MaritalStatus { get; set; }
        public string? PwdStatus { get; set; }

        public int? NumberOfHouseholdMembers { get; set; }

        public int? Male { get; set; }
        public int? Female { get; set; }

        public string? NameOfPlaceWhereTheRoadWasRehabilitated { get; set; }

        public string? ContactNumber { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
