using System.ComponentModel.DataAnnotations;

namespace sacpapi.Models
{
    public class GeneralActivity
    {
        [Key]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int OrganisationId { get; set; }
        public int SubComponentId { get; set; }
        public int InterventionId { get; set; }
        public int ActivityTypeId { get; set; }
        public int ActivityId { get; set; }
        public int ActiveStatusId { get; set; }
        public int DistrictId { get; set; }
        public string Site { get; set; } = string.Empty;
        public string SiteType { get; set; } = string.Empty;
        public int WaterpointId { get; set; }
        public int IrrigationSchemeId { get; set; }
        public int LeadFacilitatorId { get; set; }
        public string Details { get; set; } = string.Empty;
        public string IssuesComments { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTime UpdatedDate { get; set; }
    }
}
