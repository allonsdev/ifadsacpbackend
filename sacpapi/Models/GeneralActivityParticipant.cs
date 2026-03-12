using System.ComponentModel.DataAnnotations;

namespace sacpapi.Models
{
    public class GeneralActivityParticipant
    {
        [Key]
        public int Id { get; set; } = 0;
        public int GeneralActivityId { get; set; } = 0;
        public int ParticipantTypeId { get; set; } = 0;
        public int ParticipantId { get; set; } = 0;
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
