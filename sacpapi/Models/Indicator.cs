using System.ComponentModel.DataAnnotations;

namespace sacpapi.Models
{
    public class Indicator
    {
        [Key]
        public int Id { get; set; }
        public int IndicatorTypeId { get; set; }
        public int IndicatorCategoryId { get; set; }
        public int ObjectiveId { get; set; } = 0;
        public int OutcomeId { get; set; } = 0;
        public int OutputId { get; set; } = 0;
        public int ActivityId { get; set; } =  0;
        public string Name { get; set; } = string.Empty;
        public string Definition { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int UnitOfMeasurementId { get; set; }
        public int BaselineValue { get; set; }
        public int DataSourceId { get; set; }
        public int DataCollectionMethodId { get; set; }
        public int ToolId { get; set; }
        public int DataCollectionFrequencyId { get; set; }
        public string ResponsibleParty { get; set; } = string.Empty;
        public int ProgramTargetValue { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTime UpdatedDate { get; set; }
    }
}
