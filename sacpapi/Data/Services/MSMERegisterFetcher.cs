using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using sacpapi.Data.Services;
using sacpapi.Models;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Azure;

public class MSMERegisterFetcher : BackgroundService
{
    private readonly ILogger<MSMERegisterFetcher> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly string KoboApiBaseUrl = "https://kc.kobotoolbox.org/";
    private readonly string KoboApiEndpoint = "api/v1/data/1382777";
    private readonly string AccessToken = "b087792e3787c84913d1e62f7795bb5f133e5e59";
    public MSMERegisterFetcher(ILogger<MSMERegisterFetcher> logger, IHttpClientFactory httpClientFactory, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;

        _httpClientFactory = httpClientFactory;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var scopedService = scope.ServiceProvider.GetRequiredService<IMSMERegisterEnterprises>();

                        client.BaseAddress = new Uri(KoboApiBaseUrl);
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Token", AccessToken);

                        HttpResponseMessage response = await client.GetAsync(KoboApiEndpoint, stoppingToken);

                        if (response.IsSuccessStatusCode)
                        {
                            var jsonString = await response.Content.ReadAsStringAsync();
                            var jsonArray = JsonConvert.DeserializeObject<dynamic[]>(jsonString);
                            var successful = 0;
                            var unsuccessful = 0;

                            foreach (var obj in jsonArray)
                            {
                                try
                                {
                                    MSMERegisterEnterprise enterprise = new MSMERegisterEnterprise();
                                    enterprise.koboEnterpriseId = obj.ContainsKey("_id") ? obj["_id"] : null;
                                    enterprise.formhub_uuid = obj.ContainsKey("formhub/uuid") ? obj["formhub/uuid"] : null;
                                    enterprise.today = obj.ContainsKey("today") ? (DateTime?)Convert.ChangeType(obj["today"], typeof(DateTime)) : null;
                                    enterprise.province = obj.ContainsKey("province") ? obj["province"] : null;
                                    enterprise.district = obj.ContainsKey("district") ? obj["district"] : null;
                                    enterprise.ward = obj.ContainsKey("ward") ? obj["ward"] : null;
                                    enterprise.buscenter = obj.ContainsKey("buscenter") ? obj["buscenter"] : null;
                                    enterprise.enumerator = obj.ContainsKey("enumerator") ? obj["enumerator"] : null;
                                    enterprise.nameofbus = obj.ContainsKey("nameofbus") ? obj["nameofbus"] : null;
                                    enterprise.tradingname = obj.ContainsKey("tradingname") ? obj["tradingname"] : null;
                                    enterprise.regstatus = obj.ContainsKey("regstatus") ? obj["regstatus"] : null;
                                    enterprise.namecontact = obj.ContainsKey("namecontact") ? obj["namecontact"] : null;
                                    enterprise.address = obj.ContainsKey("address") ? obj["address"] : null;
                                    enterprise.phone = obj.ContainsKey("phone") ? obj["phone"] : null;
                                    enterprise.email = obj.ContainsKey("email") ? obj["email"] : null;
                                    enterprise.legalstatus = obj.ContainsKey("legalstatus") ? obj["legalstatus"] : null;
                                    enterprise.yearest = obj.ContainsKey("yearest") ? obj["yearest"] : null;
                                    enterprise.staffno = obj.ContainsKey("staffno") ? obj["staffno"] : null;
                                    enterprise.femalesno = obj.ContainsKey("femalesno") ? obj["femalesno"] : null;
                                    enterprise.maleno = obj.ContainsKey("maleno") ? obj["maleno"] : null;
                                    enterprise.youthno = obj.ContainsKey("youthno") ? obj["youthno"] : null;
                                    enterprise.annualbudget = obj.ContainsKey("annualbudget") ? obj["annualbudget"] : null;
                                    enterprise.networks = obj.ContainsKey("networks") ? obj["networks"] : null;
                                    enterprise.nobranches = obj.ContainsKey("nobranches") ? obj["nobranches"] : null;
                                    enterprise.location = obj.ContainsKey("location") ? obj["location"] : null;
                                    enterprise.prductoff1 = obj.ContainsKey("prductoff1") ? obj["prductoff1"] : null;
                                    enterprise.cusprod1 = obj.ContainsKey("cusprod1") ? obj["cusprod1"] : null;
                                    enterprise.prod1 = obj.ContainsKey("prod1") ? obj["prod1"] : null;
                                    enterprise.serv1 = obj.ContainsKey("serv1") ? obj["serv1"] : null;
                                    enterprise.rawmat1 = obj.ContainsKey("rawmat1") ? obj["rawmat1"] : null;
                                    enterprise.prductoff2 = obj.ContainsKey("prductoff2") ? obj["prductoff2"] : null;
                                    enterprise.cusprod2 = obj.ContainsKey("cusprod2") ? obj["cusprod2"] : null;
                                    enterprise.prod2 = obj.ContainsKey("prod2") ? obj["prod2"] : null;
                                    enterprise.serv2 = obj.ContainsKey("serv2") ? obj["serv2"] : null;
                                    enterprise.rawmat2 = obj.ContainsKey("rawmat2") ? obj["rawmat2"] : null;
                                    enterprise.prductoff3 = obj.ContainsKey("prductoff3") ? obj["prductoff3"] : null;
                                    enterprise.cusprod3 = obj.ContainsKey("cusprod3") ? obj["cusprod3"] : null;
                                    enterprise.prod3 = obj.ContainsKey("prod3") ? obj["prod3"] : null;
                                    enterprise.serv = obj.ContainsKey("serv") ? obj["serv"] : null;
                                    enterprise.rawmat = obj.ContainsKey("rawmat") ? obj["rawmat"] : null;
                                    enterprise.assetdes1 = obj.ContainsKey("assetdes1") ? obj["assetdes1"] : null;
                                    enterprise.currentvalue1 = obj.ContainsKey("currentvalue1") ? obj["currentvalue1"] : null;
                                    enterprise.condition1 = obj.ContainsKey("condition1") ? obj["condition1"] : null;
                                    enterprise.assetdes2 = obj.ContainsKey("assetdes2") ? obj["assetdes2"] : null;
                                    enterprise.currentvalue2 = obj.ContainsKey("currentvalue2") ? obj["currentvalue2"] : null;
                                    enterprise.condition2 = obj.ContainsKey("condition2") ? obj["condition2"] : null;
                                    enterprise.assetdes3 = obj.ContainsKey("assetdes3") ? obj["assetdes3"] : null;
                                    enterprise.currentvalue3 = obj.ContainsKey("currentvalue3") ? obj["currentvalue3"] : null;
                                    enterprise.condition3 = obj.ContainsKey("condition3") ? obj["condition3"] : null;
                                    enterprise.techcap = obj.ContainsKey("techcap") ? obj["techcap"] : null;
                                    enterprise.valuechain1 = obj.ContainsKey("valuechain1") ? obj["valuechain1"] : null;
                                    enterprise.locat1 = obj.ContainsKey("locat1") ? obj["locat1"] : null;
                                    enterprise.funds1 = obj.ContainsKey("funds1") ? obj["funds1"] : null;
                                    enterprise.source1 = obj.ContainsKey("source1") ? obj["source1"] : null;
                                    enterprise.periodacti1 = obj.ContainsKey("periodacti1") ? obj["periodacti1"] : null;
                                    enterprise.type1 = obj.ContainsKey("type1") ? obj["type1"] : null;
                                    enterprise.valuechain2 = obj.ContainsKey("valuechain2") ? obj["valuechain2"] : null;
                                    enterprise.locat2 = obj.ContainsKey("locat2") ? obj["locat2"] : null;
                                    enterprise.funds2 = obj.ContainsKey("funds2") ? obj["funds2"] : null;
                                    enterprise.source2 = obj.ContainsKey("source2") ? obj["source2"] : null;
                                    enterprise.periodacti2 = obj.ContainsKey("periodacti2") ? obj["periodacti2"] : null;
                                    enterprise.type2 = obj.ContainsKey("type2") ? obj["type2"] : null;
                                    enterprise.valuechain3 = obj.ContainsKey("valuechain3") ? obj["valuechain3"] : null;
                                    enterprise.locat3 = obj.ContainsKey("locat3") ? obj["locat3"] : null;
                                    enterprise.funds3 = obj.ContainsKey("funds3") ? obj["funds3"] : null;
                                    enterprise.source3 = obj.ContainsKey("source3") ? obj["source3"] : null;
                                    enterprise.periodacti3 = obj.ContainsKey("periodacti3") ? obj["periodacti3"] : null;
                                    enterprise.type3 = obj.ContainsKey("type3") ? obj["type3"] : null;
                                    enterprise.modela = obj.ContainsKey("modela") ? obj["modela"] : null;
                                    enterprise.genderenv = obj.ContainsKey("genderenv") ? obj["genderenv"] : null;
                                    enterprise.sacpbene = obj.ContainsKey("sacpbene") ? obj["sacpbene"] : null;
                                    enterprise.capacity = obj.ContainsKey("capacity") ? obj["capacity"] : null;
                                    enterprise.plans = obj.ContainsKey("plans") ? obj["plans"] : null;
                                    enterprise.procure = obj.ContainsKey("procure") ? obj["procure"] : null;
                                    enterprise.fin = obj.ContainsKey("fin") ? obj["fin"] : null;
                                    enterprise.org1 = obj.ContainsKey("org1") ? obj["org1"] : null;
                                    enterprise.fon1 = obj.ContainsKey("fon1") ? obj["fon1"] : null;
                                    enterprise.org2 = obj.ContainsKey("org2") ? obj["org2"] : null;
                                    enterprise.fon2 = obj.ContainsKey("fon2") ? obj["fon2"] : null;
                                    enterprise.org3 = obj.ContainsKey("org3") ? obj["org3"] : null;
                                    enterprise.fon3 = obj.ContainsKey("fon3") ? obj["fon3"] : null;
                                    enterprise._version_ = obj.ContainsKey("_version_") ? obj["_version_"] : null;
                                    enterprise.meta_instanceID = obj.ContainsKey("meta/instanceID") ? obj["meta/instanceID"] : null;
                                    enterprise._xform_id_string = obj.ContainsKey("_xform_id_string") ? obj["_xform_id_string"] : null;
                                    enterprise._uuid = obj.ContainsKey("_uuid") ? obj["_uuid"] : null;
                                    enterprise._submission_time = obj.ContainsKey("_submission_time") ? (DateTime?)Convert.ChangeType(obj["_submission_time"], typeof(DateTime)) : null;
                                    enterprise._submitted_by = obj.ContainsKey("_submitted_by") ? obj["_submitted_by"] : null;
                                    enterprise.start = obj.ContainsKey("start") ? (DateTime?)Convert.ChangeType(obj["start"], typeof(DateTime)) : null;
                                    enterprise.end = obj.ContainsKey("end") ? (DateTime?)Convert.ChangeType(obj["end"], typeof(DateTime)) : null;

                                    scopedService.Add(enterprise);
                                    successful += 1;
                                }
                                catch (Exception)
                                {
                                    unsuccessful += 1;
                                    continue;
                                }

                            }

                            _logger.LogInformation($"Successfulrecords:{successful}, Unsuccessfulrecords:{unsuccessful}: {response.StatusCode}");
                        }
                        else
                        {
                            _logger.LogError($"Failed to fetch data from Kobo API. Status Code: {response.StatusCode}");
                        }

                        await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
            }
        }
    }
}
