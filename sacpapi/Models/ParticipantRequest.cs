namespace sacpapi.Models
{
    public class ParticipantRequest
    {
        public string? FarmerName { get; set; } = null;
            public string? Gender { get; set; } = null;
            public string IDNumber { get; set; }
            public DateTime DateOfBirth { get; set; } = DateTime.MinValue;
            public string? Disabled { get; set; } = null;
            public string? ContactNumber { get; set; } = null;
            public string? Disability { get; set; } = null;
            public string? Province { get; set; } = null;
            public string? District { get; set; } = null;
            public string? Ward { get; set; } = null;
            public string? Village { get; set; } = null;
            public string? HouseholdHeadSex { get; set; } = null;
            public string? MaritalStatus { get; set; } = null;
            public string? RelationshipToHead { get; set; } = null;
            public string? ExtensionOfficerName { get; set; } = null;
            public string? Exists { get; set; } = null;
            public string? Status { get; set; } = null;
    }
}
