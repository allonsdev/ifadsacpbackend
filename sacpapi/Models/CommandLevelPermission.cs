using System.ComponentModel.DataAnnotations;

namespace sacpapi.Models
{
    public class CommandLevelPermission
    {
        [Key]
        public int Id { get; set; }
        public int CommandId { get; set; }
        public int UserId { get; set; }
    }
}
