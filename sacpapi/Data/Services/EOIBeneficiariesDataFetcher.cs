using Newtonsoft.Json;
using sacpapi.Models;

namespace sacpapi.Data.Services
{
    public class EOIBeneficiariesDataFetcher : BackgroundService
    {
        private readonly ILogger<EOIBeneficiariesDataFetcher> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly string KoboApiBaseUrl = "https://kc.kobotoolbox.org/";
        private readonly string KoboApiEndpoint = "api/v1/data/1609604";
        private readonly string AccessToken = "b087792e3787c84913d1e62f7795bb5f133e5e59";

        public EOIBeneficiariesDataFetcher(ILogger<EOIBeneficiariesDataFetcher> logger, IHttpClientFactory httpClientFactory, IServiceScopeFactory serviceScopeFactory)
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
                            var scopedService = scope.ServiceProvider.GetRequiredService<IEOIBeneficiaries>();

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
                                        EOIBeneficiary beneficiary = new EOIBeneficiary();
                                        if (obj != null)
                                        {
                                            beneficiary.KoboBeneficiaryId = obj.ContainsKey("_id") ? obj["_id"] : null;
                                            beneficiary.Province = obj.ContainsKey("province") ? obj["province"] : null;
                                            beneficiary.District = obj.ContainsKey("district") ? obj["district"] : null;
                                            beneficiary.Ward = obj.ContainsKey("ward") ? obj["ward"] : null;
                                            beneficiary.Village = obj.ContainsKey("village") ? obj["village"] : null;

                                            beneficiary.Latitude = obj.ContainsKey("_geolocation[0]") ? obj["_geolocation[0]"] : null;
                                            beneficiary.Longitude = obj.ContainsKey("_geolocation[1]") ? obj["_geolocation[1]"] : null;
                                            beneficiary.FarmerName = obj.ContainsKey("householdhead") ? obj["householdhead"] : null;
                                            beneficiary.Gender = obj.ContainsKey("gender") ? obj["gender"] : null;
                                            beneficiary.DateOfBirth = obj.ContainsKey("dob") ? obj["dob"] : null;
                                            beneficiary.IDNumber = obj.ContainsKey("idno") ? obj["idno"] : null;
                                            beneficiary.HouseholdHeadSex = obj.ContainsKey("sex") ? obj["sex"] : null;
                                            beneficiary.MaritalStatus = obj.ContainsKey("marital") ? obj["marital"] : null;

                                            beneficiary.RelationshipToHead = obj.ContainsKey("relation") ? obj["relation"] : null;
                                            beneficiary.ContactNumber = obj.ContainsKey("phone") ? obj["phone"] : null;
                                            beneficiary.ExtensionOfficerName = obj.ContainsKey("enumerator") ? obj["enumerator"] : null;

                                            beneficiary.NumHouseholdMembers = obj.ContainsKey("hhmembers") ? obj["hhmembers"] : null;
                                            beneficiary.NumFemalesUnder5 = obj.ContainsKey("female1") ? obj["female1"] : null;
                                            beneficiary.NumMalesUnder5 = obj.ContainsKey("male1") ? obj["male1"] : null;
                                            beneficiary.NumFemales5To18 = obj.ContainsKey("female2") ? obj["female2"] : null;
                                            beneficiary.NumMales5To18 = obj.ContainsKey("male2") ? obj["male2"] : null;
                                            beneficiary.NumFemales18To35 = obj.ContainsKey("female3") ? obj["female3"] : null;
                                            beneficiary.NumMales18To35 = obj.ContainsKey("male3") ? obj["male3"] : null;
                                            beneficiary.NumFemalesOver35 = obj.ContainsKey("female4") ? obj["female4"] : null;
                                            beneficiary.NumMalesOver35 = obj.ContainsKey("male4") ? obj["male4"] : null;
                                            beneficiary.NumHouseholdLabourers = obj.ContainsKey("labour") ? obj["labour"] : null;
                                            beneficiary.Disabled = obj.ContainsKey("disability") ? obj["disability"] : null;
                                            beneficiary.Disability = obj.ContainsKey("disability") ? obj["disability"] : null;
                                            beneficiary.WaterSource = obj.ContainsKey("water") ? obj["water"] : null;
                                            beneficiary.TotalArableLandHa = obj.ContainsKey("arablesizeha") ? obj["arablesizeha"] : null;
                                            beneficiary.TotalProductionArea = obj.ContainsKey("prodarabha") ? obj["prodarabha"] : null;
                                            beneficiary.CropsGrown = obj.ContainsKey("cropdry") ? obj["cropdry"] : null;
                                            beneficiary.MainCropGrown = obj.ContainsKey("maindry") ? obj["maindry"] : null;
                                            beneficiary.TotalHarvestMainCrop = obj.ContainsKey("maindryha") ? obj["maindryha"] : null;
                                            beneficiary.Unit = obj.ContainsKey("unitdry") ? obj["unitdry"] : null;
                                            beneficiary.CSATechnologiesUsed = obj.ContainsKey("csa") ? obj["csa"] : null;
                                            beneficiary.AccessToIrrigation = obj.ContainsKey("accessirr") ? obj["accessirr"] : null;

                                            beneficiary.FoodInsecurityPastYear = obj.ContainsKey("foodsec") ? obj["foodsec"] : null;
                                            beneficiary.MonthsFoodInsecurity = obj.ContainsKey("monfoodsec") ? obj["monfoodsec"] : null;
                                            beneficiary.MemberOfAgricultureGroup = obj.ContainsKey("exist") ? obj["exist"] : null;
                                            beneficiary.MainActivity = obj.ContainsKey("activgroup") ? obj["activgroup"] : null;
                                            beneficiary.MembershipPosition = obj.ContainsKey("nameapg") ? obj["nameapg"] : null;
                                            beneficiary.CanContributeToProject = obj.ContainsKey("contri") ? obj["contri"] : null;
                                            beneficiary.OwnsCattle = obj.ContainsKey("cattle") ? obj["cattle"] : null;
                                            beneficiary.CattleNo = obj.ContainsKey("cattleno") ? obj["cattleno"] : 0;
                                            beneficiary.OwnsDonkeys = obj.ContainsKey("donkeys") ? obj["donkeys"] : null;
                                            beneficiary.DonkeysNo = obj.ContainsKey("donkeysno") ? obj["donkeysno"] : 0;
                                            beneficiary.OwnsGoats = obj.ContainsKey("goats") ? obj["goats"] : null;
                                            beneficiary.GoatsNo = obj.ContainsKey("goatsno") ? obj["goatsno"] : 0;
                                            beneficiary.OwnsSheep = obj.ContainsKey("sheep") ? obj["sheep"] : null;
                                            beneficiary.SheepNo = obj.ContainsKey("sheepno") ? obj["sheepno"] : 0;
                                            beneficiary.OwnsPoultry = obj.ContainsKey("indchicken") ? obj["indchicken"] : null;
                                            beneficiary.PoultryNo = obj.ContainsKey("indchicken_001") ? obj["indchicken_001"] : 0;
                                            beneficiary.MainHouseType = obj.ContainsKey("ironsheet") ? obj["ironsheet"] : null;
                                            beneficiary.AccessToFinance = obj.ContainsKey("finance") ? obj["finance"] : null;
                                            beneficiary.FinancialSource = obj.ContainsKey("finsouce") ? obj["finsouce"] : null;
                                            beneficiary.RemittancesAmount = obj.ContainsKey("rem") ? obj["rem"] : 0;
                                            beneficiary.FoodCropSalesAmount = obj.ContainsKey("foodcrops") ? obj["foodcrops"] : 0;
                                            beneficiary.CashCropSalesAmount = obj.ContainsKey("cashcrops") ? obj["cashcrops"] : 0;
                                            beneficiary.VegetableSalesAmount = obj.ContainsKey("vegesales") ? obj["vegesales"] : 0;
                                            beneficiary.CasualLabourIncome = obj.ContainsKey("casual") ? obj["casual"] : 0;

                                            beneficiary.LivestockSalesIncome = obj.ContainsKey("livesales") ? obj["livesales"] : 0;
                                            beneficiary.LivestockProductsIncome = obj.ContainsKey("liveprod") ? obj["liveprod"] : 0;
                                            beneficiary.SkilledTradeIncome = obj.ContainsKey("uuid") ? obj["uuid"] : 0;
                                            beneficiary.OwnBusinessIncome = obj.ContainsKey("ownbus") ? obj["ownbus"] : 0;
                                            beneficiary.PensionIncome = obj.ContainsKey("pension") ? obj["pension"] : 0;
                                            beneficiary.FormalSalariesIncome = obj.ContainsKey("formalsalar") ? obj["formalsalar"] : 0;
                                            beneficiary.FishingIncome = obj.ContainsKey("fishing") ? obj["fishing"] : 0;
                                            beneficiary.SmallScaleMiningIncome = obj.ContainsKey("uuid") ? obj["uuid"] : 0;
                                            beneficiary.GovSocialTransfersIncome = obj.ContainsKey("uuid") ? obj["uuid"] : 0;
                                            beneficiary.NonStateSocialTransfersIncome = obj.ContainsKey("uuid") ? obj["uuid"] : 0;
                                            beneficiary.CrossBorderTradeIncome = obj.ContainsKey("uuid") ? obj["uuid"] : 0;
                                            beneficiary.RentalsIncome = obj.ContainsKey("uuid") ? obj["uuid"] : 0;
                                            beneficiary.OtherIncomeSources = obj.ContainsKey("uuid") ? obj["uuid"] : null;
                                            beneficiary.OtherIncomeAmount = obj.ContainsKey("uuid") ? obj["uuid"] : 0;
                                            beneficiary.WillingToParticipateInTrainings = obj.ContainsKey("patrain") ? obj["patrain"] : null;
                                            //trainben
                                            beneficiary.AvgCultivatedLandArea = obj.ContainsKey("land") ? obj["land"] : null;
                                            beneficiary.NumHouseholdLaborersForAgri = obj.ContainsKey("labour_001") ? obj["labour_001"] : null;
                                            beneficiary.MainCropAvgProduction = obj.ContainsKey("production") ? obj["production"] : null;
                                            beneficiary.AvgMonthlyIncomePerHousehold = obj.ContainsKey("income") ? obj["income"] : null;
                                            beneficiary.FarmerCategory = obj.ContainsKey("category") ? obj["category"] : null;
                                            beneficiary.BeneficialTrainings = obj.ContainsKey("trainben") ? obj["trainben"] : null;

                                            beneficiary.FormHubUuid = obj.ContainsKey("formhub/uuid") ? obj["formhub/uuid"] : null;
                                            beneficiary.Start = obj.ContainsKey("start") ? obj["start"] : null;
                                            beneficiary.End = obj.ContainsKey("end") ? obj["end"] : null;
                                            beneficiary.Today = obj.ContainsKey("today") ? obj["today"] : null;
                                            beneficiary.Version = obj.ContainsKey("__version__") ? obj["__version__"] : null;
                                            beneficiary.MetaInstanceID = obj.ContainsKey("meta/instanceID") ? obj["meta/instanceID"] : null;
                                            beneficiary.XFormIdString = obj.ContainsKey("_xform_id_string") ? obj["_xform_id_string"] : null;
                                            beneficiary.Uuid = obj.ContainsKey("_uuid") ? obj["_uuid"] : null;
                                            beneficiary.Status = obj.ContainsKey("_status") ? obj["_status"] : null;
                                            beneficiary.SubmissionTime = obj.ContainsKey("_submission_time") ? obj["_submission_time"] : null;
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

                            await Task.Delay(TimeSpan.FromDays(14), stoppingToken);
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
}
