using System.ComponentModel.DataAnnotations;

namespace sacpapi.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public required int StaffMemberId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public int FailedPasswordAttemptCount { get; set; } = 0;
        public bool? IsApproved { get; set; }
        public bool? IsLockedOut { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? LastActivityDate { get; set; }
        public DateTime? LastLockoutDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? LastPasswordChangeDate { get; set; }
        public string? PasswordQuestion { get; set; }
        public string? PasswordQuestionAnswer { get; set; }
        public bool? PasswordExpires { get; set; } = false;
        public int? PasswordExpirationDays { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
        public string? UpdatedBy { get; set; }
        public bool? Deleted { get; set; } = false;
    }
}