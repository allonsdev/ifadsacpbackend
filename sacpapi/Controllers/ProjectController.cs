using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sacpapi.Data;
using sacpapi.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace sacpapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("allbugdgets")]
        public async Task<IActionResult> GetAllRecords()
        {
            var records = await _context.FinancialRecords.ToListAsync();
            return Ok(records);
        }
        [HttpPost("addbudgets")]
        public async Task<IActionResult> AddRecord([FromBody] FinancialRecord record)
        {
            _context.FinancialRecords.Add(record);
            await _context.SaveChangesAsync();
            return Ok(record);
        }

        // **3. Get a single record by ID**
        [HttpGet("allbugdgets/{id}")]
        public async Task<IActionResult> GetRecordById(int id)
        {
            var record = await _context.FinancialRecords.FindAsync(id);
            if (record == null)
                return NotFound("Record not found.");
            return Ok(record);
        }

        // **4. Update a record**
        [HttpPut("allbugdgetsupdate/{id}")]
        public async Task<IActionResult> UpdateRecord(int id, [FromBody] FinancialRecord record)
        {
            if (id != record.Id)
                return BadRequest("ID mismatch.");

            var existingRecord = await _context.FinancialRecords.FindAsync(id);
            if (existingRecord == null)
                return NotFound("Record not found.");

            existingRecord.FinancialYear = record.FinancialYear;
            existingRecord.TotalBudget = record.TotalBudget;
            existingRecord.TotalReceived = record.TotalReceived;
            existingRecord.TotalSpent = record.TotalSpent;
            existingRecord.Balance = record.Balance;

            await _context.SaveChangesAsync();
            return Ok("Record updated successfully.");
        }

        // **5. Delete a record**
        [HttpDelete("deletebudget/{id}")]
        public async Task<IActionResult> DeleteRecord(int id)
        {
            var record = await _context.FinancialRecords.FindAsync(id);
            if (record == null)
                return NotFound("Record not found.");

            _context.FinancialRecords.Remove(record);
            await _context.SaveChangesAsync();
            return Ok("Record deleted successfully.");
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetById(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Project updatedProject)
        {
            if (id != updatedProject.Id)
            {
                return BadRequest();
            }

            _context.Entry(updatedProject).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        [HttpGet("logframe")]
        public async Task<ActionResult<object>> GetData()
        {
            var objectives = await _context.Objectives.ToListAsync();

            var rootNode = new List<object>();

            foreach (var objective in objectives)
            {
                var outcomeNodes = new List<object>();

                var outcomes = await _context.Outcomes
                    .Where(ou => ou.ObjectiveId == objective.Id)
                    .ToListAsync();

                foreach (var outcome in outcomes)
                {
                    var outputNodes = new List<object>();

                    var outputs = await _context.Outputs
                        .Where(oo => oo.OutcomeId == outcome.Id)
                        .ToListAsync();

                    foreach (var output in outputs)
                    {
                        var activityNodes = new List<object>();

                        var activities = await _context.Activities
                            .Where(a => a.OutputId == output.Id)
                            .Select(a => new
                            {
                                name = a.Name
                            })
                            .ToListAsync();

                        foreach (var activity in activities)
                        {
                            activityNodes.Add(new
                            {
                                data = new
                                {
                                    activity = activity.name
                                }
                            });
                        }

                        outputNodes.Add(new
                        {
                            data = new
                            {
                                output = output.Name
                            },
                            children = activityNodes
                        });
                    }

                    outcomeNodes.Add(new
                    {
                        data = new
                        {
                            outcome = outcome.Name
                        },
                        children = outputNodes
                    });
                }

                rootNode.Add(new
                {
                    data = new
                    {
                        objective = objective.Name
                    },
                    children = outcomeNodes
                });
            }

            return Json(rootNode);
        }


        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}
