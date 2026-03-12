using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sacpapi.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Acronym { get; set; }
        public string? ActiveStatus { get; set; }
        public string? FinalGoalStatement { get; set; }
        public string? ProjectManager { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public DateOnly EvaluationDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalProjectBudget { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalReceived { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalSpent { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }
        public string? BeneficiaryDescription { get; set; }
        public string? StakeholderDescription { get; set; }
        public int TargetedNoOfDirectBeneficiaries { get; set; }
        public int TargetedNoOfGroups { get; set; }
        public int TargetedNoOfMsmes { get; set; }
        public int Men { get; set; }
        public int Women { get; set; }
        public int Youth { get; set; }
        public int Plwd { get; set; }
        public int Vcle { get; set; }
        public int Mhhh { get; set; }
        public int Whhh { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class FinancialRecord
    {
        public int Id { get; set; } // Primary Key
        public string FinancialYear { get; set; }  // e.g., "2024-25"
        public decimal TotalBudget { get; set; }
        public decimal TotalReceived { get; set; }
        public decimal TotalSpent { get; set; }
        public decimal Balance { get; set; }
    }


}
