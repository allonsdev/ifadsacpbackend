using Microsoft.AspNetCore.Http;

namespace sacpapi.Models
{
    public class FileUploadModel
    {
        public IFormFile File { get; set; }
        public string? FileType { get; set; }
        public string? Author { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public string? AuthorOrganization { get; set; }
        public bool ApplySecurity { get; set; }
        public string? ApprovalStatus { get; set; }
        public int ObjectId { get; set; }
        public int ObjectTypeId { get; set; }
        public string? CreatedBy { get; set; }
        public List<int>? FilePermissions { get; set; }
    }
}
