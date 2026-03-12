using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using sacpapi.Data;
using sacpapi.Models;
using System.Data;

namespace sacpapi.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class AchievementsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string? connectionString;


        public AchievementsController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnectionString");
        }
        [HttpGet]
        public IActionResult Index()
        {
            int achievement = 0;
            var measurementArray = new int[] { 1, 2, 3, 4, 5 };

            var targetsWithMatchingUnits = (from target in _context.IndicatorTargets
                                            join indicator in _context.Indicators
                                            on target.IndicatorId equals indicator.Id
                                            where measurementArray.Contains(indicator.UnitOfMeasurementId)
                                            select target)
                                            .ToList();

            foreach (var target in targetsWithMatchingUnits)
            {
                var indicator = _context.Indicators
                    .Where(i => i.Id == target.IndicatorId)
                    .FirstOrDefault();
                string sql = "";
                if (indicator != null)
                {
                    switch (indicator.DataCollectionFrequencyId)
                    {
                        case 1:
                            sql = $"SELECT COUNT(dbo.GeneralActivityParticipants.ParticipantId) AS Count, dbo.IndicatorTargets.Id\r\nFROM     dbo.ActivitiesIndicators INNER JOIN\r\n                  dbo.IndicatorTargets ON dbo.ActivitiesIndicators.IndicatorId = dbo.IndicatorTargets.IndicatorId INNER JOIN\r\n                  dbo.GeneralActivities ON dbo.ActivitiesIndicators.ActivityId = dbo.GeneralActivities.ActivityId INNER JOIN\r\n                  dbo.GeneralActivityParticipants ON dbo.GeneralActivities.Id = dbo.GeneralActivityParticipants.GeneralActivityId WHERE\r\n    dbo.GeneralActivityParticipants.ParticipantTypeId = {indicator.UnitOfMeasurementId}\r\n    AND MONTH(dbo.GeneralActivities.EndDate) = {AchievementHelper.GetMonthValue(target.Month)} AND (dbo.IndicatorTargets.Id = {target.Id})\r\nGROUP BY dbo.IndicatorTargets.Id";
                            break;

                        case 2:
                            sql = $"SELECT COUNT(dbo.GeneralActivityParticipants.ParticipantId) AS Count, dbo.IndicatorTargets.Id\r\nFROM     dbo.ActivitiesIndicators INNER JOIN\r\n                  dbo.IndicatorTargets ON dbo.ActivitiesIndicators.IndicatorId = dbo.IndicatorTargets.IndicatorId INNER JOIN\r\n                  dbo.GeneralActivities ON dbo.ActivitiesIndicators.ActivityId = dbo.GeneralActivities.ActivityId INNER JOIN\r\n                  dbo.GeneralActivityParticipants ON dbo.GeneralActivities.Id = dbo.GeneralActivityParticipants.GeneralActivityId WHERE\r\n    dbo.GeneralActivityParticipants.ParticipantTypeId = {indicator.UnitOfMeasurementId}\r\n    AND MONTH(dbo.GeneralActivities.EndDate) IN ({AchievementHelper.GetMonthsForQuarter(target.Quarter)}) AND (dbo.IndicatorTargets.Id = {target.Id})\r\nGROUP BY dbo.IndicatorTargets.Id";
                            break;

                        case 3:
                            sql = $"SELECT COUNT(dbo.GeneralActivityParticipants.ParticipantId) AS Count, dbo.IndicatorTargets.Id\r\nFROM     dbo.ActivitiesIndicators INNER JOIN\r\n                  dbo.IndicatorTargets ON dbo.ActivitiesIndicators.IndicatorId = dbo.IndicatorTargets.IndicatorId INNER JOIN\r\n                  dbo.GeneralActivities ON dbo.ActivitiesIndicators.ActivityId = dbo.GeneralActivities.ActivityId INNER JOIN\r\n                  dbo.GeneralActivityParticipants ON dbo.GeneralActivities.Id = dbo.GeneralActivityParticipants.GeneralActivityId WHERE\r\n    dbo.GeneralActivityParticipants.ParticipantTypeId = {indicator.UnitOfMeasurementId}\r\n    AND YEAR(dbo.GeneralActivities.EndDate) = {target.FinancialYear} AND (dbo.IndicatorTargets.Id = {target.Id})\r\nGROUP BY dbo.IndicatorTargets.Id";
                            break;

                        default:

                            break;
                    }
                    using (var connection = new SqlConnection(connectionString))
                    {

                        using (var command = new SqlCommand(sql, connection))
                        {
                            // Open the connection before executing the command
                            connection.Open();
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read()) // Check if there is a row returned
                                {
                                    achievement = Convert.ToInt32(reader["Count"].ToString());
                                    connection.Close();
                                    target.Achievement = achievement;
                                    _context.SaveChanges();
                                }
                            }
                        }
                    }

                }
            }

            return Ok();
        }

        [HttpPost("{indicatorId}")]
        public IActionResult Create(int indicatorId, [FromBody] List<int> ids)
        {
            try
            {
                if (ids != null && ids.Count > 0)
                {

                    foreach (var id in ids)
                    {
                        var exists = _context.ActivitiesIndicators.FirstOrDefault(g => g.ActivityId == id && g.IndicatorId == indicatorId);

                        if (exists == null)
                        {
                            ActivityIndicator activityIndicator = new ActivityIndicator();
                            activityIndicator.ActivityId = id;
                            activityIndicator.IndicatorId = indicatorId;

                            _context.ActivitiesIndicators.Add(activityIndicator);
                        }

                        continue;
                    }

                    _context.SaveChanges();

                    return StatusCode(201);

                }

                return StatusCode(500);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
    public static class AchievementHelper
    {        
        public static int GetMonthValue(string month)
        {
            switch (month.ToLower())
            {
                case "january":
                    return 1;
                case "february":
                    return 2;
                case "march":
                    return 3;
                case "april":
                    return 4;
                case "may":
                    return 5;
                case "june":
                    return 6;
                case "july":
                    return 7;
                case "august":
                    return 8;
                case "september":
                    return 9;
                case "october":
                    return 10;
                case "november":
                    return 11;
                case "december":
                    return 12;
                default:
                    throw new ArgumentException("Invalid month input");
            }
        }
        public static string GetMonthsForQuarter(string quarter)
        {
            switch (quarter.ToLower())
            {
                case "q1":
                    return "1, 2, 3";
                case "q2":
                    return "4, 5, 6";
                case "q3":
                    return "7, 8, 9";
                case "q4":
                    return "10, 11, 12";
                default:
                    throw new ArgumentException("Invalid quarter input");
            }
        }
    }
}
