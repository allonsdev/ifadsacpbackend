using System.ComponentModel.DataAnnotations;

namespace sacpapi.Models
{
    public class FilePermission
    {
        [Key]
        public int Id { get; set; }
        public int FileId { get; set; }
        public int StaffId { get; set; }
    }
}
