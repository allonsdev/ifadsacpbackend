using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sacpapi.Data;
using sacpapi.Models;
using System.Text.RegularExpressions;

namespace sacpapi.Controllers
{
    [Route("/[controller]")]
    public class DashboardController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string? connectionString;
     
        public DashboardController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnectionString");
        }

        [HttpGet]
        public IActionResult index()
        {
                // Fetch the data as a list
                var beneficiaries = _context.vwBenefeciariesUnion
                    .Select(b => new vwBenefeciariesUnion
                    {
                        HouseholdID = b.HouseholdID,
                        Province = b.Province,
                        District = b.District,
                        Ward = b.Ward,
                        Village = b.Village,
                        Name = b.Name,
                        Surname = b.Surname,
                        Sex = b.Sex,
                        Age = b.Age,
                        DateOfBirth = b.DateOfBirth,
                        ContactNumber = b.ContactNumber,
                        DisabilityStatus = b.DisabilityStatus,
                        SexOfHousehold = b.SexOfHousehold
                    })
                    .ToList(); // Ensure you call ToList to get a list


            var beneficiaries2 = _context.Beneficiaryv3
                .Select(b => new BeneficiaryV3
                {
                    HouseholdIdentifierNumber = b.HouseholdIdentifierNumber,
                    Province = b.Province,
                    District = b.District,
                    Ward = b.Ward,
                    Village = b.Village,
                    Name = b.Name,
                    Surname = b.Surname,
                    Sex = b.Sex,
                    Age = b.Age,
                    DateOfBirth = b.DateOfBirth,
                    ContactNumber = b.ContactNumber,
                    DisabilityStatus = b.DisabilityStatus,
                    YouthStatus = b.YouthStatus,  // Keep the existing value for potential use later
                    SexOfHousehold = b.SexOfHousehold,
                    LandSize = b.LandSize,
                    ValueChain = b.ValueChain,
                    APGGroup = b.APGGroup
                })
                .ToList(); // Ensure you call ToList to get a list

            // Calculate counts
            var beneficiariesCount = beneficiaries.Count;
                var maleCount = beneficiaries.Count(b => b.Sex == "Male");
                var femaleCount = beneficiaries.Count(b => b.Sex == "Female");
                var distinctAPGGroups = beneficiaries2.Select(b => b.APGGroup).Distinct().Count();
                var disabilityCount = beneficiaries.Count(b => b.DisabilityStatus == "Yes");
            var youthCount = beneficiaries.Count(b => b.Age <= 35);  // Age 35 or below is considered youth
            var nonYouthCount = beneficiaries.Count(b => b.Age > 35);
            var maleHeadedCount = beneficiaries.Count(b =>
                !string.IsNullOrEmpty(b.SexOfHousehold) && Regex.IsMatch(b.SexOfHousehold, @"\bMale\b", RegexOptions.IgnoreCase));

            var femaleHeadedCount = beneficiaries.Count(b =>
                !string.IsNullOrEmpty(b.SexOfHousehold) && Regex.IsMatch(b.SexOfHousehold, @"\b(Women|Female)\b", RegexOptions.IgnoreCase));



            var distinctValueChainsCount = _context.Beneficiaryv3
    .Select(b => b.ValueChain) // Select only the ValueChain column
    .Distinct() // Get unique value chains
    .Count(); // Count the distinct value chains

            var project = _context.Projects.FirstOrDefault(project => project.Id == 1);
            var beneficiariesTarget = project.TargetedNoOfDirectBeneficiaries;
            var groupsTarget = project.TargetedNoOfGroups;
            var msmesTarget = project.TargetedNoOfMsmes;
            var maleTarget = project.Men;
            var femaleTarget = project.Women;
            var youthTarget = project.Youth;
            var plwdtarget = project.Plwd;
            var vcletarget = project.Vcle;
            var whhtarget = project.Whhh;

            var result = new

            {
                disabilityCount= disabilityCount,
                distinctAPGGroups=distinctAPGGroups,
                femaleHeadedCount=femaleHeadedCount,
                maleHeadedCount=maleHeadedCount,
                beneficiaries = beneficiariesCount,
                beneficiariesTarget = beneficiariesTarget,
                vcle=distinctValueChainsCount,
                groups = distinctAPGGroups,
                groupsTarget = groupsTarget,
                vcletarget = vcletarget,
                whhtarget = whhtarget,
                plwdtarget = plwdtarget,
                msmes = 0,
                msmesTarget = msmesTarget,
                male = maleCount,
                maleTarget = maleTarget,
                female = femaleCount,
                femaleTarget = femaleTarget,
                youth = youthCount,
                youthTarget = youthTarget
            };

            return Json(result);
        }



        [HttpGet("district")]
        public IActionResult reachbyDistrict()
        {
            var CountByDistrict = _context.vwBenefeciariesUnion
                .GroupBy(u => new { District = u.District ?? "Unknown", Sex = u.Sex ?? "Unknown" }) // Handle null values
                .Select(group => new
                {
                    district = group.Key.District,
                    sex = group.Key.Sex,
                    count = group.Count()
                })
                .ToList();

            var groupedData = CountByDistrict
                .GroupBy(item => item.district)
                .Select(group => new
                {
                    district = group.Key,
                    male = group.Where(x => x.sex == "Male").Sum(x => x.count),
                    female = group.Where(x => x.sex == "Female").Sum(x => x.count),
                    other = group.Where(x => x.sex != "Male" && x.sex != "Female").Sum(x => x.count)
                })
                .ToList();

            return Json(groupedData);
        }

        [HttpGet("province")]
        public IActionResult reachbyProvince()
        {
            var CountByProvince = _context.vwBenefeciariesUnion
                .GroupBy(u => new { Province = u.Province ?? "Unknown", Sex = u.Sex ?? "Unknown" }) // Handle null values
                .Select(group => new
                {
                    province = group.Key.Province,
                    sex = group.Key.Sex,
                    count = group.Count()
                })
                .ToList();

            var groupedData = CountByProvince
                .GroupBy(item => item.province)
                .Select(group => new
                {
                    province = group.Key,
                    male = group.Where(x => x.sex == "Male").Sum(x => x.count),
                    female = group.Where(x => x.sex == "Female").Sum(x => x.count),
                    other = group.Where(x => x.sex != "Male" && x.sex != "Female").Sum(x => x.count)
                })
                .ToList();

            return Json(groupedData);
        }


        [HttpGet("valuechain")]
        public IActionResult reachbyVC()
        {
            var CountByDistrict = _context.Beneficiaries
            .GroupBy(u => u.District)
            .Select(group => new
            {
                district = group.Key,
                reach = group.Count()
            })
            .ToList();

            return Json(CountByDistrict);
        }
    }
}
