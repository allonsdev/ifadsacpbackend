using System.ComponentModel.DataAnnotations;

namespace sacpapi.Models
{
    public class SubMenuPermission
    {
        [Key]
        public int Id { get; set; }
        public int MenuItemId { get; set; }
        public int UserId { get; set; }
    }
}
