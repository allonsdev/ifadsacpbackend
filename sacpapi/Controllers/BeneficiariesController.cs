using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using sacpapi.Data;
using sacpapi.Data.Services;
using sacpapi.Models;
using System.Text.Json;

namespace sacpapi.Controllers
{
    [Route("/[controller]")]
    public class BeneficiariesController : Controller
    {
        private IBeneficiaries _service;
        private readonly ApplicationDbContext _context;
        public BeneficiariesController(IBeneficiaries service, ApplicationDbContext context)
        {
            _service = service;
            _context = context;
        }
        public IActionResult Index()
        {
            var data = _service.GetAll();
            return Json(data);
        }

        [HttpGet("array")]
        public IActionResult IdsArray()
        {
            var data = _context.Beneficiaries.Select(b=>b.KoboBeneficiaryId).ToArray();
            return Json(data);
        }

        [HttpPost]
        public IActionResult Create([FromBody] List<int> ids)
        {
            try
            {
                if (ids != null && ids.Count > 0)
                {
                    var successful = 0;
                    var unsuccessful = 0;

                    foreach (var id in ids)
                    {
                        try
                        {
                            var existingEntity = _context.EOIBeneficiaries.FirstOrDefault(e => e.KoboBeneficiaryId == id);

                            if (existingEntity != null)
                            {

                                Beneficiary newEntity = new Beneficiary();
                                newEntity.KoboBeneficiaryId = existingEntity.KoboBeneficiaryId;
                                newEntity.Province = existingEntity.Province;
                                newEntity.District = existingEntity.District;
                                newEntity.Ward = existingEntity.Ward;
                                newEntity.Village = existingEntity.Village;
                                newEntity.Latitude = existingEntity.Latitude;
                                newEntity.Longitude = existingEntity.Longitude;
                                newEntity.FarmerName = existingEntity.FarmerName;
                                newEntity.Gender = existingEntity.Gender;
                                if (existingEntity.DateOfBirth.HasValue)
                                {
                                    newEntity.DateOfBirth = existingEntity.DateOfBirth.Value;
                                }
                                else
                                {
                                    newEntity.DateOfBirth = DateTime.MinValue;
                                }
                                newEntity.IDNumber = existingEntity.IDNumber;
                                newEntity.HouseholdHeadSex = existingEntity.HouseholdHeadSex;
                                newEntity.MaritalStatus = existingEntity.MaritalStatus;
                                newEntity.RelationshipToHead = existingEntity.RelationshipToHead;
                                newEntity.ContactNumber = existingEntity.ContactNumber;
                                newEntity.ExtensionOfficerName = existingEntity.ExtensionOfficerName;
                                newEntity.NumHouseholdMembers = existingEntity.NumHouseholdMembers;
                                newEntity.NumFemalesUnder5 = existingEntity.NumFemalesUnder5;
                                newEntity.NumMalesUnder5 = existingEntity.NumMalesUnder5;
                                newEntity.NumFemales5To18 = existingEntity.NumFemales5To18;
                                newEntity.NumMales5To18 = existingEntity.NumMales5To18;
                                newEntity.NumFemales18To35 = existingEntity.NumFemales18To35;
                                newEntity.NumMales18To35 = existingEntity.NumMales18To35;
                                newEntity.NumFemalesOver35 = existingEntity.NumFemalesOver35;
                                newEntity.NumMalesOver35 = existingEntity.NumMalesOver35;
                                newEntity.NumHouseholdLabourers = existingEntity.NumHouseholdLabourers;
                                newEntity.Disabled = existingEntity.Disabled;
                                newEntity.Disability = existingEntity.Disability;
                                newEntity.WaterSource = existingEntity.WaterSource;
                                newEntity.TotalArableLandHa = existingEntity.TotalArableLandHa;
                                newEntity.TotalProductionArea = existingEntity.TotalProductionArea;
                                newEntity.CropsGrown = existingEntity.CropsGrown;
                                newEntity.MainCropGrown = existingEntity.MainCropGrown;
                                newEntity.TotalHarvestMainCrop = existingEntity.TotalHarvestMainCrop;
                                newEntity.Unit = existingEntity.Unit;
                                newEntity.CSATechnologiesUsed = existingEntity.CSATechnologiesUsed;
                                newEntity.AccessToIrrigation = existingEntity.AccessToIrrigation;
                                newEntity.FoodInsecurityPastYear = existingEntity.FoodInsecurityPastYear;
                                newEntity.MonthsFoodInsecurity = existingEntity.MonthsFoodInsecurity;
                                newEntity.MemberOfAgricultureGroup = existingEntity.MemberOfAgricultureGroup;
                                newEntity.MainActivity = existingEntity.MainActivity;
                                newEntity.MembershipPosition = existingEntity.MembershipPosition;
                                newEntity.CanContributeToProject = existingEntity.CanContributeToProject;
                                newEntity.OwnsCattle = existingEntity.OwnsCattle;
                                newEntity.CattleNo = existingEntity.CattleNo;
                                newEntity.OwnsDonkeys = existingEntity.OwnsDonkeys;
                                newEntity.DonkeysNo = existingEntity.DonkeysNo;
                                newEntity.OwnsGoats = existingEntity.OwnsGoats;
                                newEntity.GoatsNo = existingEntity.GoatsNo;
                                newEntity.OwnsSheep = existingEntity.OwnsSheep;
                                newEntity.SheepNo = existingEntity.SheepNo;
                                newEntity.OwnsPoultry = existingEntity.OwnsPoultry;
                                newEntity.PoultryNo = existingEntity.PoultryNo;
                                newEntity.MainHouseType = existingEntity.MainHouseType;
                                newEntity.AccessToFinance = existingEntity.AccessToFinance;
                                newEntity.FinancialSource = existingEntity.FinancialSource;
                                newEntity.RemittancesAmount = existingEntity.RemittancesAmount;
                                newEntity.FoodCropSalesAmount = existingEntity.FoodCropSalesAmount;
                                newEntity.CashCropSalesAmount = existingEntity.CashCropSalesAmount;
                                newEntity.VegetableSalesAmount = existingEntity.VegetableSalesAmount;
                                newEntity.CasualLabourIncome = existingEntity.CasualLabourIncome;
                                newEntity.LivestockSalesIncome = existingEntity.LivestockSalesIncome;
                                newEntity.LivestockProductsIncome = existingEntity.LivestockProductsIncome;
                                newEntity.SkilledTradeIncome = existingEntity.SkilledTradeIncome;
                                newEntity.OwnBusinessIncome = existingEntity.OwnBusinessIncome;
                                newEntity.PensionIncome = existingEntity.PensionIncome;
                                newEntity.FormalSalariesIncome = existingEntity.FormalSalariesIncome;
                                newEntity.FishingIncome = existingEntity.FishingIncome;
                                newEntity.SmallScaleMiningIncome = existingEntity.SmallScaleMiningIncome;
                                newEntity.GovSocialTransfersIncome = existingEntity.GovSocialTransfersIncome;
                                newEntity.NonStateSocialTransfersIncome = existingEntity.NonStateSocialTransfersIncome;
                                newEntity.CrossBorderTradeIncome = existingEntity.CrossBorderTradeIncome;
                                newEntity.RentalsIncome = existingEntity.RentalsIncome;
                                newEntity.OtherIncomeSources = existingEntity.OtherIncomeSources;
                                newEntity.OtherIncomeAmount = existingEntity.OtherIncomeAmount;
                                newEntity.WillingToParticipateInTrainings = existingEntity.WillingToParticipateInTrainings;
                                newEntity.AvgCultivatedLandArea = existingEntity.AvgCultivatedLandArea;
                                newEntity.NumHouseholdLaborersForAgri = existingEntity.NumHouseholdLaborersForAgri;
                                newEntity.MainCropAvgProduction = existingEntity.MainCropAvgProduction;
                                newEntity.AvgMonthlyIncomePerHousehold = existingEntity.AvgMonthlyIncomePerHousehold;
                                newEntity.FarmerCategory = existingEntity.FarmerCategory;
                                newEntity.BeneficialTrainings = existingEntity.BeneficialTrainings;
                                newEntity.FormHubUuid = existingEntity.FormHubUuid;
                                newEntity.Start = existingEntity.Start;
                                newEntity.End = existingEntity.End;
                                newEntity.Today = existingEntity.Today;
                                newEntity.Version = existingEntity.Version;
                                newEntity.MetaInstanceID = existingEntity.MetaInstanceID;
                                newEntity.XFormIdString = existingEntity.XFormIdString;
                                newEntity.Uuid = existingEntity.Uuid;
                                newEntity.Attachments = existingEntity.Attachments;
                                newEntity.Status = existingEntity.Status;
                                newEntity.SubmissionTime = existingEntity.SubmissionTime;

                                _service.Add(newEntity);
                                successful += 1;
                            }
                            else
                            {
                                unsuccessful += 1;
                            }
                        }
                        catch (Exception)
                        {
                            unsuccessful += 1;
                            continue;
                        }
                    }

                    var result = new
                    {
                        SuccessfulRecords = successful,
                        UnsuccessfulRecords = unsuccessful,
                        TotalRecords = successful + unsuccessful
                    };

                    return Ok(result);
                }
                else
                {
                    return StatusCode(500, "Empty ID list received");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var beneficiary = _context.Beneficiaries.Find(id);
            if (beneficiary == null)
            {
                return NotFound();
            }
            try
            {
                _context.Beneficiaries.Remove(beneficiary);
                _context.SaveChanges();

                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

