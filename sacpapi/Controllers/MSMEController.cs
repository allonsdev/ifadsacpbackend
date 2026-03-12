using Microsoft.AspNetCore.Mvc;
using sacpapi.Data;
using sacpapi.Data.Services;
using sacpapi.Models;

namespace sacpapi.Controllers
{
    [Route("/[controller]")]
    public class MSMEController : Controller
    {
        private readonly IMSMEs _service;
        private readonly ApplicationDbContext _context;
        public MSMEController(IMSMEs service, ApplicationDbContext context)
        {
            _service = service;
            _context = context;
        }
        public IActionResult Index()
        {
            var data = _service.GetAll();
            if (!(data.Any())) return Json(data);
            return Json(data);
        }
        [HttpGet("array")]
        public IActionResult IdsArray()
        {
            var data = _context.MSMERegister.Select(b => b.koboEnterpriseId).ToArray();
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
                            var existingEntity = _context.MSMERegister.FirstOrDefault(e => e.koboEnterpriseId == id);

                            if (existingEntity != null)
                            {

                                MSME newEntity = new MSME();
                                newEntity.koboEnterpriseId = existingEntity.koboEnterpriseId;
                                newEntity.formhub_uuid = existingEntity.formhub_uuid;
                                newEntity.today = existingEntity.today;
                                newEntity.start = existingEntity.start;
                                newEntity.end = existingEntity.end;
                                newEntity.province = existingEntity.province;
                                newEntity.district = existingEntity.district;
                                newEntity.ward = existingEntity.ward;
                                newEntity.buscenter = existingEntity.buscenter;
                                newEntity.enumerator = existingEntity.enumerator;
                                newEntity.nameofbus = existingEntity.nameofbus;
                                newEntity.tradingname = existingEntity.tradingname;
                                newEntity.regstatus = existingEntity.regstatus;
                                newEntity.namecontact = existingEntity.namecontact;
                                newEntity.address = existingEntity.address;
                                newEntity.phone = existingEntity.phone;
                                newEntity.email = existingEntity.email;
                                newEntity.legalstatus = existingEntity.legalstatus;
                                newEntity.yearest = existingEntity.yearest;
                                newEntity.staffno = existingEntity.staffno;
                                newEntity.femalesno = existingEntity.femalesno;
                                newEntity.maleno = existingEntity.maleno;
                                newEntity.youthno = existingEntity.youthno;
                                newEntity.annualbudget = existingEntity.annualbudget;
                                newEntity.networks = existingEntity.networks;
                                newEntity.nobranches = existingEntity.nobranches;
                                newEntity.location = existingEntity.location;
                                newEntity.prductoff1 = existingEntity.prductoff1;
                                newEntity.cusprod1 = existingEntity.cusprod1;
                                newEntity.prod1 = existingEntity.prod1;
                                newEntity.serv1 = existingEntity.serv1;
                                newEntity.rawmat1 = existingEntity.rawmat1;
                                newEntity.prductoff2 = existingEntity.prductoff2;
                                newEntity.cusprod2 = existingEntity.cusprod2;
                                newEntity.prod2 = existingEntity.prod2;
                                newEntity.serv2 = existingEntity.serv2;
                                newEntity.rawmat2 = existingEntity.rawmat2;
                                newEntity.prductoff3 = existingEntity.prductoff3;
                                newEntity.cusprod3 = existingEntity.cusprod3;
                                newEntity.prod3 = existingEntity.prod3;
                                newEntity.serv = existingEntity.serv;
                                newEntity.rawmat = existingEntity.rawmat;
                                newEntity.assetdes1 = existingEntity.assetdes1;
                                newEntity.currentvalue1 = existingEntity.currentvalue1;
                                newEntity.condition1 = existingEntity.condition1;
                                newEntity.assetdes2 = existingEntity.assetdes2;
                                newEntity.currentvalue2 = existingEntity.currentvalue2;
                                newEntity.condition2 = existingEntity.condition2;
                                newEntity.assetdes3 = existingEntity.assetdes3;
                                newEntity.currentvalue3 = existingEntity.currentvalue3;
                                newEntity.condition3 = existingEntity.condition3;
                                newEntity.techcap = existingEntity.techcap;
                                newEntity.valuechain1 = existingEntity.valuechain1;
                                newEntity.locat1 = existingEntity.locat1;
                                newEntity.funds1 = existingEntity.funds1;
                                newEntity.source1 = existingEntity.source1;
                                newEntity.periodacti1 = existingEntity.periodacti1;
                                newEntity.type1 = existingEntity.type1;
                                newEntity.valuechain2 = existingEntity.valuechain2;
                                newEntity.locat2 = existingEntity.locat2;
                                newEntity.funds2 = existingEntity.funds2;
                                newEntity.source2 = existingEntity.source2;
                                newEntity.periodacti2 = existingEntity.periodacti2;
                                newEntity.type2 = existingEntity.type2;
                                newEntity.valuechain3 = existingEntity.valuechain3;
                                newEntity.locat3 = existingEntity.locat3;
                                newEntity.funds3 = existingEntity.funds3;
                                newEntity.source3 = existingEntity.source3;
                                newEntity.periodacti3 = existingEntity.periodacti3;
                                newEntity.type3 = existingEntity.type3;
                                newEntity.modela = existingEntity.modela;
                                newEntity.genderenv = existingEntity.genderenv;
                                newEntity.sacpbene = existingEntity.sacpbene;
                                newEntity.capacity = existingEntity.capacity;
                                newEntity.plans = existingEntity.plans;
                                newEntity.procure = existingEntity.procure;
                                newEntity.fin = existingEntity.fin;
                                newEntity.org1 = existingEntity.org1;
                                newEntity.fon1 = existingEntity.fon1;
                                newEntity.org2 = existingEntity.org2;
                                newEntity.fon2 = existingEntity.fon2;
                                newEntity.org3 = existingEntity.org3;
                                newEntity.fon3 = existingEntity.fon3;
                                newEntity._version_ = existingEntity._version_;
                                newEntity.meta_instanceID = existingEntity.meta_instanceID;
                                newEntity._xform_id_string = existingEntity._xform_id_string;
                                newEntity._uuid = existingEntity._uuid;
                                newEntity._submission_time = existingEntity._submission_time;
                                newEntity._submitted_by = existingEntity._submitted_by;

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
            var msmes = _context.MSMEs.Find(id);
            if (msmes == null)
            {
                return NotFound();
            }
            try
            {
                _context.MSMEs.Remove(msmes);
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
