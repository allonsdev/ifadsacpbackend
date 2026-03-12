using System.ComponentModel.DataAnnotations;

namespace sacpapi.Models
{
    public class UserUserGroup
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int UserGroupId { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}