using System.ComponentModel.DataAnnotations;

namespace sacpapi.Models
{
    public class Files
    {
        [Key]
        public int Id { get; set; }
        public string? FileType { get; set; }
        public string? Author { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public string? FilePath { get; set; }
        public string? FileExtension { get; set; }
        public string? AuthorOrganization { get; set; }
        public bool ApplySecurity { get; set; }
        public string? ApprovalStatus { get; set; }
        public int? ActionedBy { get; set; }
        public string? ToBeActionedBy { get; set; }
        public string? ContentType { get; set; }
        public int ObjectId { get; set; }
        public int ObjectTypeId { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}
