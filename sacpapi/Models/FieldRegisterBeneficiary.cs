using System;
using System.ComponentModel.DataAnnotations;

namespace sacpapi.Models
{
    public class FieldRegisterBeneficiary
    {
        [Key]
        public int Id { get; set; } = 0;

        public int KoboBeneficiaryId { get; set; } = 0;

        [Display(Name = "Formhub UUID")]
        public string? FormhubUuid { get; set; } = string.Empty;

        public DateTime Date { get; set; } = DateTime.MinValue;

        [Display(Name = "Name of Activity")]
        public string? ActivityName { get; set; } = string.Empty;

        public string? Province { get; set; } = string.Empty;

        public string? District { get; set; } = string.Empty;

        public string? Ward { get; set; } = string.Empty;

        public string? Enumerator { get; set; } = string.Empty;

        [Display(Name = "Household Head")]
        public string? HouseholdHead { get; set; }

        [Display(Name = "Sex of Household Head")]
        public string? HouseholdHeadSex { get; set; }

        [Display(Name = "Name of Your Organization")]
        public string? OrganizationName { get; set; } = string.Empty;

        [Display(Name = "Date of Birth")]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "ID Number")]
        public string? IdNumber { get; set; } = string.Empty;

        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        public string? MetaInstanceID { get; set; } = string.Empty;

        public string? XFormIdString { get; set; } = string.Empty;

        public string? Uuid { get; set; } = string.Empty;

        public string? SubmissionStatus { get; set; }

        [Display(Name = "Submission Time")]
        public DateTime? SubmissionTime { get; set; } = DateTime.MinValue;

        [Display(Name = "Submitted By")]
        public string? SubmittedBy { get; set; } = string.Empty;
    }
}
