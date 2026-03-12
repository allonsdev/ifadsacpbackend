using System.ComponentModel.DataAnnotations;

namespace sacpapi.Models
{
    public class StakeholderParticipants
    {
        [Key]
        public int Id { get; set; }
        public string ?Gtid { get; set; }
        public string? NameOfParticipant { get; set; }
        public string? Sex { get; set; }   // "M" or "F"
        public string? Organisation { get; set; }
        public string? Position { get; set; }
        public string? ContactNumber { get; set; }
        public string? EmailAddress { get; set; }
        public string? UploadedBy { get; set; }

        // Automatically sets current date/time when a new object is created
        public DateTime UploadedDate { get; set; } = DateTime.Now;
    }
}
