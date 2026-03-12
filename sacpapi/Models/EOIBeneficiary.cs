using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace sacpapi.Models
{
    public class EOIBeneficiary
    {
        [Key]
        public int Id { get; set; }
        public int KoboBeneficiaryId { get; set; } = 0;
        public string? Province { get; set; } = null;
        public string? District { get; set; } = null;
        public string? Ward { get; set; } = null;
        public string? Village { get; set; } = null;
        public double? Latitude { get; set; } = null;
        public double? Longitude { get; set; } = null;
        public string? FarmerName { get; set; } = null;
        public string? Gender { get; set; } = null;
        public DateTime? DateOfBirth { get; set; } = DateTime.MinValue;
        public string? IDNumber { get; set; } = null;
        public string? HouseholdHeadSex { get; set; } = null;
        public string? MaritalStatus { get; set; } = null;
        public string? RelationshipToHead { get; set; } = null;
        public string? ContactNumber { get; set; } = null;
        public string? ExtensionOfficerName { get; set; } = null;
        public int? NumHouseholdMembers { get; set; } = 0;
        public int? NumFemalesUnder5 { get; set; } = 0;
        public int? NumMalesUnder5 { get; set; } = 0;
        public int? NumFemales5To18 { get; set; } = 0;
        public int? NumMales5To18 { get; set; } = 0;
        public int? NumFemales18To35 { get; set; } = 0;
        public int? NumMales18To35 { get; set; } = 0;
        public int? NumFemalesOver35 { get; set; } = 0;
        public int? NumMalesOver35 { get; set; } = 0;
        public int? NumHouseholdLabourers { get; set; } = 0;
        public string? Disabled { get; set; } = null;
        public string? Disability { get; set; } = null;
        public string? WaterSource { get; set; } = null;
        public double? TotalArableLandHa { get; set; } = 0;
        public double? TotalProductionArea { get; set; } = 0;
        public string? CropsGrown { get; set; } = null;
        public string? MainCropGrown { get; set; } = null;
        public double? TotalHarvestMainCrop { get; set; } = 0;
        public string? Unit { get; set; } = null;
        public string? CSATechnologiesUsed { get; set; } = null;
        public string? AccessToIrrigation { get; set; } = null;
        public string? FoodInsecurityPastYear { get; set; } = null;
        public string? MonthsFoodInsecurity { get; set; } = null;
        public string? MemberOfAgricultureGroup { get; set; } = null;
        public string? MainActivity { get; set; } = null;
        public string? MembershipPosition { get; set; } = null;
        public string? CanContributeToProject { get; set; } = null;
        public string? OwnsCattle { get; set; } = null;
        public int? CattleNo { get; set; } = 0;
        public string? OwnsDonkeys { get; set; } = null;
        public int? DonkeysNo { get; set; } = 0;
        public string? OwnsGoats { get; set; } = null;
        public int? GoatsNo { get; set; } = 0;
        public string? OwnsSheep { get; set; } = null;
        public int? SheepNo { get; set; } = 0;
        public string? OwnsPoultry { get; set; } = null;
        public int? PoultryNo { get; set; } = 0;
        public string? MainHouseType { get; set; } = null;
        public string? AccessToFinance { get; set; } = null;
        public string? FinancialSource { get; set; } = null;
        public int? RemittancesAmount { get; set; } = 0;
        public int? FoodCropSalesAmount { get; set; } = 0;
        public int? CashCropSalesAmount { get; set; } = 0;
        public int? VegetableSalesAmount { get; set; } = 0;
        public int? CasualLabourIncome { get; set; } = 0;
        public int? LivestockSalesIncome { get; set; } = 0;
        public int? LivestockProductsIncome { get; set; } = 0;
        public int? SkilledTradeIncome { get; set; } = 0;
        public int? OwnBusinessIncome { get; set; } = 0;
        public int? PensionIncome { get; set; } = 0;
        public int? FormalSalariesIncome { get; set; } = 0;
        public int? FishingIncome { get; set; } = 0;
        public int? SmallScaleMiningIncome { get; set; } = 0;
        public int? GovSocialTransfersIncome { get; set; } = 0;
        public int? NonStateSocialTransfersIncome { get; set; } = 0;
        public int? CrossBorderTradeIncome { get; set; } = 0;
        public int? RentalsIncome { get; set; } = 0;
        public string? OtherIncomeSources { get; set; } = null;
        public int? OtherIncomeAmount { get; set; } = 0;
        //specify activity
        public string? WillingToParticipateInTrainings { get; set; } = null;
        public string? AvgCultivatedLandArea { get; set; } = null;
        public string? NumHouseholdLaborersForAgri { get; set; } = null;
        public string? MainCropAvgProduction { get; set; } = null;
        public string? AvgMonthlyIncomePerHousehold { get; set; } = null;
        public string? FarmerCategory { get; set; } = null;
        public string? BeneficialTrainings { get; set; } = null;
        public string? FormHubUuid { get; set; } = null;
        public DateTime? Start { get; set; } = DateTime.MinValue;
        public DateTime? End { get; set; } = DateTime.MinValue;
        public DateTime? Today { get; set; } = DateTime.MinValue;
        public string? Version { get; set; } = null;
        public string? MetaInstanceID { get; set; } = null;
        public string? XFormIdString { get; set; } = null;
        public string? Uuid { get; set; } = null;
        public string? Attachments { get; set; } = null;
        public string? Status { get; set; } = null;
        public DateTime? SubmissionTime { get; set; } = DateTime.MinValue;
    }
}