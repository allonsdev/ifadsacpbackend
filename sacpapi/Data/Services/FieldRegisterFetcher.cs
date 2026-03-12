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

public class FieldRegisterFetcher : BackgroundService
{
    private readonly ILogger<FieldRegisterFetcher> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly string KoboApiBaseUrl = "https://kc.kobotoolbox.org/";
    private readonly string KoboApiEndpoint = "api/v1/data/1357136";
    private readonly string AccessToken = "b087792e3787c84913d1e62f7795bb5f133e5e59";
    public FieldRegisterFetcher(ILogger<FieldRegisterFetcher> logger, IHttpClientFactory httpClientFactory, IServiceScopeFactory serviceScopeFactory)
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
                    client.Timeout = TimeSpan.FromSeconds(200);
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var scopedService = scope.ServiceProvider.GetRequiredService<IFieldRegisterBeneficiaries>();

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
                                    FieldRegisterBeneficiary beneficiary = new FieldRegisterBeneficiary();
                                    if (obj != null)
                                    {
                                        // Perform null checks before accessing and assigning properties
                                        beneficiary.KoboBeneficiaryId = obj.ContainsKey("_id") ? obj["_id"] : null;
                                        beneficiary.FormhubUuid = obj.ContainsKey("formhub/uuid") ? obj["formhub/uuid"]?.ToString() : null;

                                        if (obj.ContainsKey("date") && DateTime.TryParse(obj["date"]?.ToString(), out DateTime dateValue))
                                        {
                                            beneficiary.Date = DateTime.MinValue;
                                        }

                                        // Check and assign other properties with null checks
                                        beneficiary.ActivityName = obj.ContainsKey("Name_of_activity") ? obj["Name_of_activity"]?.ToString() : null;
                                        beneficiary.Province = obj.ContainsKey("province") ? obj["province"]?.ToString() : null;
                                        beneficiary.District = obj.ContainsKey("district") ? obj["district"]?.ToString() : null;
                                        beneficiary.Ward = obj.ContainsKey("ward") ? obj["ward"]?.ToString() : null;
                                        beneficiary.Enumerator = obj.ContainsKey("enumerator") ? obj["enumerator"]?.ToString() : null;
                                        beneficiary.HouseholdHead = obj.ContainsKey("householdhead") ? obj["householdhead"]?.ToString() : null;
                                        beneficiary.HouseholdHeadSex = obj.ContainsKey("sexhhh") ? obj["sexhhh"]?.ToString() : null;
                                        beneficiary.OrganizationName = obj.ContainsKey("Name_of_your_organisation") ? obj["Name_of_your_organisation"]?.ToString() : null;

                                        if (obj.ContainsKey("dob") && DateTime.TryParse(obj["dob"]?.ToString(), out DateTime dobValue))
                                        {
                                            beneficiary.DateOfBirth = DateTime.MinValue;
                                        }

                                        beneficiary.IdNumber = obj.ContainsKey("idno") ? obj["idno"]?.ToString() : null;
                                        beneficiary.PhoneNumber = obj.ContainsKey("phonenumber") ? obj["phonenumber"]?.ToString() : null;
                                        beneficiary.MetaInstanceID = obj.ContainsKey("meta/instanceID") ? obj["meta/instanceID"]?.ToString() : null;
                                        beneficiary.XFormIdString = obj.ContainsKey("_xform_id_string") ? obj["_xform_id_string"]?.ToString() : null;
                                        beneficiary.Uuid = obj.ContainsKey("_uuid") ? obj["_uuid"]?.ToString() : null;
                                        beneficiary.SubmissionStatus = obj.ContainsKey("_status") ? obj["_status"]?.ToString() : null;

                                        if (obj.ContainsKey("_submission_time") && DateTime.TryParse(obj["_submission_time"]?.ToString(), out DateTime submissionTimeValue))
                                        {
                                            beneficiary.SubmissionTime = null;
                                        }

                                        beneficiary.SubmittedBy = obj.ContainsKey("_submitted_by") ? obj["_submitted_by"]?.ToString() : null;
                                    }

                                    scopedService.Add(beneficiary);
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

                        await Task.Delay(TimeSpan.FromDays(5), stoppingToken);
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
