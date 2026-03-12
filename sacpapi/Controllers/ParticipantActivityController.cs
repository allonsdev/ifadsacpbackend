using Microsoft.AspNetCore.Mvc;

namespace sacpapi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using sacpapi.Data.Services;
    using System.Data;

    [ApiController]
    [Route("api/participant-activity")]
    public class ParticipantActivityController : ControllerBase
    {
        private readonly ParticipantActivityService _service;

        public ParticipantActivityController(ParticipantActivityService service)
        {
            _service = service;
        }

        // 🔹 JSON Dataset (Angular consumption)
        [HttpGet("json")]
        public async Task<IActionResult> GetJson()
        {
            DataTable table = await _service.GetParticipantActivityAsync();
            string json = _service.DataTableToJson(table);

            return Content(json, "application/json");
        }

        // 🔹 Excel Download
        [HttpGet("excel")]
        public async Task<IActionResult> DownloadExcel()
        {
            DataTable table = await _service.GetParticipantActivityAsync();
            byte[] file = _service.GenerateExcel(table);

            return File(
                file,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "ParticipantActivity.xlsx"
            );
        }
    }

}
