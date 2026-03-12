using System.ComponentModel.DataAnnotations;

namespace sacpapi.Models
{
    public class StaffMember
    {
        [Key]
        public int Id { get; set; } = 0;
        public required string  FirstName { get; set; }
        public required string Surname { get; set; }
        public string StaffFullName => $"{FirstName} {Surname}";
        public required string NationalIdNumber { get; set; }
        public string Sex { get; set; } = string.Empty;
        public string? Address { get; set; }
        public int DistrictId { get; set; } = 0;
        public string? ContactNo { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public int OrganisationId { get; set; } = 0;
        public int DepartmentId { get; set; } = 0;
        public int PositionId { get; set; } = 0;
        public string? CreatedBy { get; set; }= string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string? UpdatedBy { get; set; } = string.Empty;
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}