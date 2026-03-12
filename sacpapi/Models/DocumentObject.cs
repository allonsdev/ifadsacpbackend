using System.ComponentModel.DataAnnotations;

namespace sacpapi.Models
{
    public class DocumentObject
    {
        [Key]
        public int Id { get; set; }
        public int FileId { get; set; }
        public int ObjectType { get; set; }
        public int ObjectId { get; set; }
    }
}
