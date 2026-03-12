using System.ComponentModel.DataAnnotations;

namespace sacpapi.Models
{
    public class GroupMember
    {
        [Key]
        public int Id { get; set; } = 0;
        public int GroupId { get; set; } = 0;
        public int BeneficiaryId { get; set; } = 0;
    }
}
