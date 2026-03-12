using Newtonsoft.Json;
using sacpapi.Models;
using sacpapi.Data;
using Microsoft.Extensions.DependencyInjection; // Add this for creating scope

public class FocusGroupDiscussionDataFetcher : BackgroundService
{
    private readonly ILogger<GroupsDataFetcher> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly string KoboApiBaseUrl = "https://kc.kobotoolbox.org/";
    private readonly string KoboApiEndpoint = "api/v1/data/1289975";
    private readonly string AccessToken = "b087792e3787c84913d1e62f7795bb5f133e5e59";

    public FocusGroupDiscussionDataFetcher(ILogger<GroupsDataFetcher> logger, IHttpClientFactory httpClientFactory, IServiceScopeFactory serviceScopeFactory)
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
                    client.BaseAddress = new Uri(KoboApiBaseUrl);
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Token", AccessToken);

                    HttpResponseMessage response = await client.GetAsync(KoboApiEndpoint, stoppingToken);

                    if (response.IsSuccessStatusCode)
                    {
                        try
                        {
                            using (var scope = _serviceScopeFactory.CreateScope())
                            {
                                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                                var jsonString = await response.Content.ReadAsStringAsync();
                                var jsonArray = JsonConvert.DeserializeObject<dynamic[]>(jsonString);
                                var successful = 0;
                                var unsuccessful = 0;

                                foreach (var obj in jsonArray)
                                {
                                    try
                                    {

                                        if (obj != null)
                                        {
                                            FocusGroupDiscussion focusGroupDiscussion = new FocusGroupDiscussion();
                                            focusGroupDiscussion.today = obj.ContainsKey("today") ? Convert.ToDateTime(obj["today"]) : default(DateTime);
                                            focusGroupDiscussion.point = obj.ContainsKey("point") ? obj["point"] : null;
                                            focusGroupDiscussion.enumerator = obj.ContainsKey("enumerator") ? obj["enumerator"] : null;
                                            focusGroupDiscussion.province = obj.ContainsKey("province") ? obj["province"] : null;
                                            focusGroupDiscussion.district = obj.ContainsKey("district") ? obj["district"] : null;
                                            focusGroupDiscussion.ward = obj.ContainsKey("ward") ? obj["ward"] : null;
                                            focusGroupDiscussion.village = obj.ContainsKey("village") ? obj["village"] : null;
                                            focusGroupDiscussion.Name_of_the_Village_2 = obj.ContainsKey("Name_of_the_Village_2") ? obj["Name_of_the_Village_2"] : null;
                                            focusGroupDiscussion.Name_of_the_Village_3 = obj.ContainsKey("Name_of_the_Village_3") ? obj["Name_of_the_Village_3"] : null;
                                            focusGroupDiscussion.Name_of_the_Village_4 = obj.ContainsKey("Name_of_the_Village_4") ? obj["Name_of_the_Village_4"] : null;
                                            focusGroupDiscussion.Name_of_the_Village_5 = obj.ContainsKey("Name_of_the_Village_5") ? obj["Name_of_the_Village_5"] : null;
                                            focusGroupDiscussion.Name_of_the_Village_6 = obj.ContainsKey("Name_of_the_Village_6") ? obj["Name_of_the_Village_6"] : null;
                                            focusGroupDiscussion.Name_of_the_Village_7 = obj.ContainsKey("Name_of_the_Village_7") ? obj["Name_of_the_Village_7"] : null;
                                            focusGroupDiscussion.Name_of_the_Village_8 = obj.ContainsKey("Name_of_the_Village_8") ? obj["Name_of_the_Village_8"] : null;
                                            focusGroupDiscussion.Name_of_the_Village_9 = obj.ContainsKey("Name_of_the_Village_9") ? obj["Name_of_the_Village_9"] : null;
                                            focusGroupDiscussion.Name_of_the_Village_10 = obj.ContainsKey("Name_of_the_Village_10") ? obj["Name_of_the_Village_10"] : null;
                                            focusGroupDiscussion.noparticipants = obj.ContainsKey("noparticipants") ? Convert.ToInt32(obj["noparticipants"]) : 0;
                                            focusGroupDiscussion.youth = obj.ContainsKey("youth") ? Convert.ToInt32(obj["youth"]) : 0;
                                            focusGroupDiscussion.youth_001 = obj.ContainsKey("youth_001") ? Convert.ToInt32(obj["youth_001"]) : 0;
                                            focusGroupDiscussion.females = obj.ContainsKey("females") ? Convert.ToInt32(obj["females"]) : 0;
                                            focusGroupDiscussion.males = obj.ContainsKey("males") ? Convert.ToInt32(obj["males"]) : 0;
                                            focusGroupDiscussion.disability = obj.ContainsKey("disability") ? Convert.ToInt32(obj["disability"]) : 0;
                                            focusGroupDiscussion.intro = obj.ContainsKey("intro") ? obj["intro"] : null;
                                            focusGroupDiscussion.gen = obj.ContainsKey("gen") ? obj["gen"] : null;
                                            focusGroupDiscussion.smallirr = obj.ContainsKey("smallirr") ? obj["smallirr"] : null;
                                            focusGroupDiscussion.Size_of_irrigation_scheme_in_village_1 = obj.ContainsKey("Size_of_irrigation_scheme_in_village_1") ? obj["Size_of_irrigation_scheme_in_village_1"] : null;
                                            focusGroupDiscussion.smallirr_009 = obj.ContainsKey("smallirr_009") ? obj["smallirr_009"] : null;
                                            focusGroupDiscussion.smallirr_008 = obj.ContainsKey("smallirr_008") ? obj["smallirr_008"] : null;
                                            focusGroupDiscussion.smallirr_007 = obj.ContainsKey("smallirr_007") ? obj["smallirr_007"] : null;
                                            focusGroupDiscussion.smallirr_006 = obj.ContainsKey("smallirr_006") ? obj["smallirr_006"] : null;
                                            focusGroupDiscussion.smallirr_005 = obj.ContainsKey("smallirr_005") ? obj["smallirr_005"] : null;
                                            focusGroupDiscussion.smallirr_004 = obj.ContainsKey("smallirr_004") ? obj["smallirr_004"] : null;
                                            focusGroupDiscussion.smallirr_003 = obj.ContainsKey("smallirr_003") ? obj["smallirr_003"] : null;
                                            focusGroupDiscussion.smallirr_002 = obj.ContainsKey("smallirr_002") ? obj["smallirr_002"] : null;
                                            focusGroupDiscussion.smallirr_001 = obj.ContainsKey("smallirr_001") ? obj["smallirr_001"] : null;
                                            focusGroupDiscussion.numberirr = obj.ContainsKey("numberirr") ? obj["numberirr"] : null;
                                            focusGroupDiscussion.nameirr1 = obj.ContainsKey("nameirr1") ? obj["nameirr1"] : null;
                                            focusGroupDiscussion.nameirr2 = obj.ContainsKey("nameirr2") ? obj["nameirr2"] : null;
                                            focusGroupDiscussion.nameirr3 = obj.ContainsKey("nameirr3") ? obj["nameirr3"] : null;
                                            focusGroupDiscussion.sizeirr1 = obj.ContainsKey("sizeirr1") ? obj["sizeirr1"] : null;
                                            focusGroupDiscussion.sizeirr2 = obj.ContainsKey("sizeirr2") ? obj["sizeirr2"] : null;
                                            focusGroupDiscussion.sizeirr3 = obj.ContainsKey("sizeirr3") ? obj["sizeirr3"] : null;
                                            focusGroupDiscussion.systemused = obj.ContainsKey("systemused") ? obj["systemused"] : null;
                                            focusGroupDiscussion.systemused_piped_surface_open_channel_flow__pumped_ = obj.ContainsKey("systemused/piped_surface_open_channel_flow__pumped_") ? obj["systemused/piped_surface_open_channel_flow__pumped_"] : null;
                                            focusGroupDiscussion.systemused_overhead = obj.ContainsKey("systemused/overhead") ? obj["systemused/overhead"] : null;
                                            focusGroupDiscussion.systemused_localised = obj.ContainsKey("systemused/localised") ? obj["systemused/localised"] : null;
                                            focusGroupDiscussion.systemused_na = obj.ContainsKey("systemused/na") ? obj["systemused/na"] : null;
                                            focusGroupDiscussion.systemused_other = obj.ContainsKey("systemused/other") ? obj["systemused/other"] : null;
                                            focusGroupDiscussion.otherirr1 = obj.ContainsKey("otherirr1") ? obj["otherirr1"] : null;
                                            focusGroupDiscussion.systemused_001 = obj.ContainsKey("systemused_001") ? obj["systemused_001"] : null;
                                            focusGroupDiscussion.systemused_001_piped_surface_open_channel_flow__pumped_ = obj.ContainsKey("systemused_001/piped_surface_open_channel_flow__pumped_") ? obj["systemused_001/piped_surface_open_channel_flow__pumped_"] : null;
                                            focusGroupDiscussion.systemused_001_overhead = obj.ContainsKey("systemused_001/overhead") ? obj["systemused_001/overhead"] : null;
                                            focusGroupDiscussion.systemused_001_localised = obj.ContainsKey("systemused_001/localised") ? obj["systemused_001/localised"] : null;
                                            focusGroupDiscussion.systemused_001_na = obj.ContainsKey("systemused_001/na") ? obj["systemused_001/na"] : null;
                                            focusGroupDiscussion.systemused_001_other = obj.ContainsKey("systemused_001/other") ? obj["systemused_001/other"] : null;
                                            focusGroupDiscussion.otherirr2 = obj.ContainsKey("otherirr2") ? obj["otherirr2"] : null;
                                            focusGroupDiscussion.systemused_002 = obj.ContainsKey("systemused_002") ? obj["systemused_002"] : null;
                                            focusGroupDiscussion.systemused_002_piped_surface_open_channel_flow__pumped_ = obj.ContainsKey("systemused_002/piped_surface_open_channel_flow__pumped_") ? obj["systemused_002/piped_surface_open_channel_flow__pumped_"] : null;
                                            focusGroupDiscussion.systemused_002_overhead = obj.ContainsKey("systemused_002/overhead") ? obj["systemused_002/overhead"] : null;
                                            focusGroupDiscussion.systemused_002_localised = obj.ContainsKey("systemused_002/localised") ? obj["systemused_002/localised"] : null;
                                            focusGroupDiscussion.systemused_002_na = obj.ContainsKey("systemused_002/na") ? obj["systemused_002/na"] : null;
                                            focusGroupDiscussion.systemused_002_other = obj.ContainsKey("systemused_002/other") ? obj["systemused_002/other"] : null;
                                            focusGroupDiscussion.otherirr3 = obj.ContainsKey("otherirr3") ? obj["otherirr3"] : null;
                                            focusGroupDiscussion.challengs = obj.ContainsKey("challengs") ? obj["challengs"] : null;
                                            focusGroupDiscussion.challengs_001 = obj.ContainsKey("challengs_001") ? obj["challengs_001"] : null;
                                            focusGroupDiscussion.challengs_002 = obj.ContainsKey("challengs_002") ? obj["challengs_002"] : null;
                                            focusGroupDiscussion.whatchale = obj.ContainsKey("whatchale") ? obj["whatchale"] : null;
                                            focusGroupDiscussion.whatchale_government = obj.ContainsKey("whatchale/government") ? obj["whatchale/government"] : null;
                                            focusGroupDiscussion.whatchale_accessinpu = obj.ContainsKey("whatchale/accessinpu") ? obj["whatchale/accessinpu"] : null;
                                            focusGroupDiscussion.whatchale_mktaccess = obj.ContainsKey("whatchale/mktaccess") ? obj["whatchale/mktaccess"] : null;
                                            focusGroupDiscussion.whatchale_irrinfra = obj.ContainsKey("whatchale/irrinfra") ? obj["whatchale/irrinfra"] : null;
                                            focusGroupDiscussion.whatchale_wateravail = obj.ContainsKey("whatchale/wateravail") ? obj["whatchale/wateravail"] : null;
                                            focusGroupDiscussion.whatchale_n_a = obj.ContainsKey("whatchale/n/a") ? obj["whatchale/n/a"] : null;
                                            focusGroupDiscussion.whatchale_otherchale = obj.ContainsKey("whatchale/otherchale") ? obj["whatchale/otherchale"] : null;
                                            focusGroupDiscussion.other1 = obj.ContainsKey("other1") ? obj["other1"] : null;
                                            focusGroupDiscussion.whatchale_001 = obj.ContainsKey("whatchale_001") ? obj["whatchale_001"] : null;
                                            focusGroupDiscussion.whatchale_001_government = obj.ContainsKey("whatchale_001/government") ? obj["whatchale_001/government"] : null;
                                            focusGroupDiscussion.whatchale_001_accessinpu = obj.ContainsKey("whatchale_001/accessinpu") ? obj["whatchale_001/accessinpu"] : null;
                                            focusGroupDiscussion.whatchale_001_mktaccess = obj.ContainsKey("whatchale_001/mktaccess") ? obj["whatchale_001/mktaccess"] : null;
                                            focusGroupDiscussion.whatchale_001_irrinfra = obj.ContainsKey("whatchale_001/irrinfra") ? obj["whatchale_001/irrinfra"] : null;
                                            focusGroupDiscussion.whatchale_001_wateravail = obj.ContainsKey("whatchale_001/wateravail") ? obj["whatchale_001/wateravail"] : null;
                                            focusGroupDiscussion.whatchale_001_n_a = obj.ContainsKey("whatchale_001/n/a") ? obj["whatchale_001/n/a"] : null;
                                            focusGroupDiscussion.whatchale_001_otherchale = obj.ContainsKey("whatchale_001/otherchale") ? obj["whatchale_001/otherchale"] : null;
                                            focusGroupDiscussion.other2 = obj.ContainsKey("other2") ? obj["other2"] : null;
                                            focusGroupDiscussion.whatchale_002 = obj.ContainsKey("whatchale_002") ? obj["whatchale_002"] : null;
                                            focusGroupDiscussion.whatchale_002_government = obj.ContainsKey("whatchale_002/government") ? obj["whatchale_002/government"] : null;
                                            focusGroupDiscussion.whatchale_002_accessinpu = obj.ContainsKey("whatchale_002/accessinpu") ? obj["whatchale_002/accessinpu"] : null;
                                            focusGroupDiscussion.whatchale_002_mktaccess = obj.ContainsKey("whatchale_002/mktaccess") ? obj["whatchale_002/mktaccess"] : null;
                                            focusGroupDiscussion.whatchale_002_irrinfra = obj.ContainsKey("whatchale_002/irrinfra") ? obj["whatchale_002/irrinfra"] : null;
                                            focusGroupDiscussion.whatchale_002_wateravail = obj.ContainsKey("whatchale_002/wateravail") ? obj["whatchale_002/wateravail"] : null;
                                            focusGroupDiscussion.whatchale_002_n_a = obj.ContainsKey("whatchale_002/n/a") ? obj["whatchale_002/n/a"] : null;
                                            focusGroupDiscussion.whatchale_002_otherchale = obj.ContainsKey("whatchale_002/otherchale") ? obj["whatchale_002/otherchale"] : null;
                                            focusGroupDiscussion.other3 = obj.ContainsKey("other3") ? obj["other3"] : null;
                                            focusGroupDiscussion.feeder = obj.ContainsKey("feeder") ? obj["feeder"] : null;
                                            focusGroupDiscussion.road = obj.ContainsKey("road") ? obj["road"] : null;
                                            focusGroupDiscussion.road_003 = obj.ContainsKey("road_003") ? obj["road_003"] : null;
                                            focusGroupDiscussion.road_002 = obj.ContainsKey("road_002") ? obj["road_002"] : null;
                                            focusGroupDiscussion.points = obj.ContainsKey("points") ? obj["points"] : null;
                                            focusGroupDiscussion.domuse = obj.ContainsKey("domuse") ? obj["domuse"] : null;
                                            focusGroupDiscussion.domuse_taps = obj.ContainsKey("domuse/taps") ? obj["domuse/taps"] : null;
                                            focusGroupDiscussion.domuse_borehole = obj.ContainsKey("domuse/borehole") ? obj["domuse/borehole"] : null;
                                            focusGroupDiscussion.domuse_wells = obj.ContainsKey("domuse/wells") ? obj["domuse/wells"] : null;
                                            focusGroupDiscussion.domuse_river = obj.ContainsKey("domuse/river") ? obj["domuse/river"] : null;
                                            focusGroupDiscussion.domuse_otherdomeuse = obj.ContainsKey("domuse/otherdomeuse") ? obj["domuse/otherdomeuse"] : null;
                                            focusGroupDiscussion.otherdomuse = obj.ContainsKey("otherdomuse") ? obj["otherdomuse"] : null;
                                            focusGroupDiscussion.time = obj.ContainsKey("time") ? obj["time"] : null;
                                            focusGroupDiscussion.rainfall = obj.ContainsKey("rainfall") ? obj["rainfall"] : null;
                                            focusGroupDiscussion.reliability = obj.ContainsKey("reliability") ? obj["reliability"] : null;
                                            focusGroupDiscussion.sani = obj.ContainsKey("sani") ? obj["sani"] : null;
                                            focusGroupDiscussion.ablution = obj.ContainsKey("ablution") ? obj["ablution"] : null;
                                            focusGroupDiscussion.ablution_002 = obj.ContainsKey("ablution_002") ? obj["ablution_002"] : null;
                                            focusGroupDiscussion.ablution_001 = obj.ContainsKey("ablution_001") ? obj["ablution_001"] : null;
                                            focusGroupDiscussion.ablution_001_007 = obj.ContainsKey("ablution_001_007") ? obj["ablution_001_007"] : null;
                                            focusGroupDiscussion.ablution_001_006 = obj.ContainsKey("ablution_001_006") ? obj["ablution_001_006"] : null;
                                            focusGroupDiscussion.ablution_001_005 = obj.ContainsKey("ablution_001_005") ? obj["ablution_001_005"] : null;
                                            focusGroupDiscussion.ablution_001_004 = obj.ContainsKey("ablution_001_004") ? obj["ablution_001_004"] : null;
                                            focusGroupDiscussion.ablution_001_003 = obj.ContainsKey("ablution_001_003") ? obj["ablution_001_003"] : null;
                                            focusGroupDiscussion.ablution_001_002 = obj.ContainsKey("ablution_001_002") ? obj["ablution_001_002"] : null;
                                            focusGroupDiscussion.ablution_001_001 = obj.ContainsKey("ablution_001_001") ? obj["ablution_001_001"] : null;
                                            focusGroupDiscussion.otherablution = obj.ContainsKey("otherablution") ? obj["otherablution"] : null;
                                            focusGroupDiscussion.env = obj.ContainsKey("env") ? obj["env"] : null;
                                            focusGroupDiscussion.environ = obj.ContainsKey("environ") ? obj["environ"] : null;
                                            focusGroupDiscussion.issues_007 = obj.ContainsKey("issues_007") ? obj["issues_007"] : null;
                                            focusGroupDiscussion.issues_007_gullies = obj.ContainsKey("issues_007/gullies") ? obj["issues_007/gullies"] : null;
                                            focusGroupDiscussion.issues_007_minepit = obj.ContainsKey("issues_007/minepit") ? obj["issues_007/minepit"] : null;
                                            focusGroupDiscussion.issues_007_otherissues = obj.ContainsKey("issues_007/otherissues") ? obj["issues_007/otherissues"] : null;
                                            focusGroupDiscussion.environ_009 = obj.ContainsKey("environ_009") ? obj["environ_009"] : null;
                                            focusGroupDiscussion.issues = obj.ContainsKey("issues") ? obj["issues"] : null;
                                            focusGroupDiscussion.issues_gullies = obj.ContainsKey("issues/gullies") ? obj["issues/gullies"] : null;
                                            focusGroupDiscussion.issues_minepit = obj.ContainsKey("issues/minepit") ? obj["issues/minepit"] : null;
                                            focusGroupDiscussion.issues_otherissues = obj.ContainsKey("issues/otherissues") ? obj["issues/otherissues"] : null;
                                            focusGroupDiscussion.environ_008 = obj.ContainsKey("environ_008") ? obj["environ_008"] : null;
                                            focusGroupDiscussion.issues_003 = obj.ContainsKey("issues_003") ? obj["issues_003"] : null;
                                            focusGroupDiscussion.issues_003_gullies = obj.ContainsKey("issues_003/gullies") ? obj["issues_003/gullies"] : null;
                                            focusGroupDiscussion.issues_003_minepit = obj.ContainsKey("issues_003/minepit") ? obj["issues_003/minepit"] : null;
                                            focusGroupDiscussion.issues_003_otherissues = obj.ContainsKey("issues_003/otherissues") ? obj["issues_003/otherissues"] : null;
                                            focusGroupDiscussion.environ_007 = obj.ContainsKey("environ_007") ? obj["environ_007"] : null;
                                            focusGroupDiscussion.issues_009 = obj.ContainsKey("issues_009") ? obj["issues_009"] : null;
                                            focusGroupDiscussion.issues_009_gullies = obj.ContainsKey("issues_009/gullies") ? obj["issues_009/gullies"] : null;
                                            focusGroupDiscussion.issues_009_minepit = obj.ContainsKey("issues_009/minepit") ? obj["issues_009/minepit"] : null;
                                            focusGroupDiscussion.issues_009_otherissues = obj.ContainsKey("issues_009/otherissues") ? obj["issues_009/otherissues"] : null;
                                            focusGroupDiscussion.environ_006 = obj.ContainsKey("environ_006") ? obj["environ_006"] : null;
                                            focusGroupDiscussion.issues_002 = obj.ContainsKey("issues_002") ? obj["issues_002"] : null;
                                            focusGroupDiscussion.issues_002_gullies = obj.ContainsKey("issues_002/gullies") ? obj["issues_002/gullies"] : null;
                                            focusGroupDiscussion.issues_002_minepit = obj.ContainsKey("issues_002/minepit") ? obj["issues_002/minepit"] : null;
                                            focusGroupDiscussion.issues_002_otherissues = obj.ContainsKey("issues_002/otherissues") ? obj["issues_002/otherissues"] : null;
                                            focusGroupDiscussion.environ_005 = obj.ContainsKey("environ_005") ? obj["environ_005"] : null;
                                            focusGroupDiscussion.issues_008 = obj.ContainsKey("issues_008") ? obj["issues_008"] : null;
                                            focusGroupDiscussion.issues_008_gullies = obj.ContainsKey("issues_008/gullies") ? obj["issues_008/gullies"] : null;
                                            focusGroupDiscussion.issues_008_minepit = obj.ContainsKey("issues_008/minepit") ? obj["issues_008/minepit"] : null;
                                            focusGroupDiscussion.issues_008_otherissues = obj.ContainsKey("issues_008/otherissues") ? obj["issues_008/otherissues"] : null;
                                            focusGroupDiscussion.environ_004 = obj.ContainsKey("environ_004") ? obj["environ_004"] : null;
                                            focusGroupDiscussion.issues_006 = obj.ContainsKey("issues_006") ? obj["issues_006"] : null;
                                            focusGroupDiscussion.issues_006_gullies = obj.ContainsKey("issues_006/gullies") ? obj["issues_006/gullies"] : null;
                                            focusGroupDiscussion.issues_006_minepit = obj.ContainsKey("issues_006/minepit") ? obj["issues_006/minepit"] : null;
                                            focusGroupDiscussion.issues_006_otherissues = obj.ContainsKey("issues_006/otherissues") ? obj["issues_006/otherissues"] : null;
                                            focusGroupDiscussion.environ_003 = obj.ContainsKey("environ_003") ? obj["environ_003"] : null;
                                            focusGroupDiscussion.issues_005 = obj.ContainsKey("issues_005") ? obj["issues_005"] : null;
                                            focusGroupDiscussion.issues_005_gullies = obj.ContainsKey("issues_005/gullies") ? obj["issues_005/gullies"] : null;
                                            focusGroupDiscussion.issues_005_minepit = obj.ContainsKey("issues_005/minepit") ? obj["issues_005/minepit"] : null;
                                            focusGroupDiscussion.issues_005_otherissues = obj.ContainsKey("issues_005/otherissues") ? obj["issues_005/otherissues"] : null;
                                            focusGroupDiscussion.environ_002 = obj.ContainsKey("environ_002") ? obj["environ_002"] : null;
                                            focusGroupDiscussion.issues_004 = obj.ContainsKey("issues_004") ? obj["issues_004"] : null;
                                            focusGroupDiscussion.issues_004_gullies = obj.ContainsKey("issues_004/gullies") ? obj["issues_004/gullies"] : null;
                                            focusGroupDiscussion.issues_004_minepit = obj.ContainsKey("issues_004/minepit") ? obj["issues_004/minepit"] : null;
                                            focusGroupDiscussion.issues_004_otherissues = obj.ContainsKey("issues_004/otherissues") ? obj["issues_004/otherissues"] : null;
                                            focusGroupDiscussion.environ_001 = obj.ContainsKey("environ_001") ? obj["environ_001"] : null;
                                            focusGroupDiscussion.issues_001 = obj.ContainsKey("issues_001") ? obj["issues_001"] : null;
                                            focusGroupDiscussion.issues_001_gullies = obj.ContainsKey("issues_001/gullies") ? obj["issues_001/gullies"] : null;
                                            focusGroupDiscussion.issues_001_minepit = obj.ContainsKey("issues_001/minepit") ? obj["issues_001/minepit"] : null;
                                            focusGroupDiscussion.issues_001_otherissues = obj.ContainsKey("issues_001/otherissues") ? obj["issues_001/otherissues"] : null;
                                            focusGroupDiscussion.otherissues = obj.ContainsKey("otherissues") ? obj["otherissues"] : null;
                                            focusGroupDiscussion.ngo = obj.ContainsKey("ngo") ? obj["ngo"] : null;
                                            focusGroupDiscussion.conl = obj.ContainsKey("conl") ? obj["conl"] : null;
                                            focusGroupDiscussion.This_note_can_be_read_out_loud = obj.ContainsKey("This_note_can_be_read_out_loud") ? obj["This_note_can_be_read_out_loud"] : null;
                                            focusGroupDiscussion.Youth_group = obj.ContainsKey("Youth_group") ? obj["Youth_group"] : null;
                                            focusGroupDiscussion.redressing = obj.ContainsKey("redressing") ? obj["redressing"] : null;
                                            focusGroupDiscussion.rankneeds = obj.ContainsKey("rankneeds") ? obj["rankneeds"] : null;
                                            focusGroupDiscussion.suggestedneeds_001 = obj.ContainsKey("suggestedneeds_001") ? obj["suggestedneeds_001"] : null;
                                            focusGroupDiscussion.Female_group = obj.ContainsKey("Female_group") ? obj["Female_group"] : null;
                                            focusGroupDiscussion.redressing_001 = obj.ContainsKey("redressing_001") ? obj["redressing_001"] : null;
                                            focusGroupDiscussion.rankneeds_001 = obj.ContainsKey("rankneeds_001") ? obj["rankneeds_001"] : null;
                                            focusGroupDiscussion.suggestedneeds = obj.ContainsKey("suggestedneeds") ? obj["suggestedneeds"] : null;
                                            focusGroupDiscussion.Male_Group = obj.ContainsKey("Male_Group") ? obj["Male_Group"] : null;
                                            focusGroupDiscussion.redressing_001_002 = obj.ContainsKey("redressing_001_002") ? obj["redressing_001_002"] : null;
                                            focusGroupDiscussion.rankneeds_002 = obj.ContainsKey("rankneeds_002") ? obj["rankneeds_002"] : null;
                                            focusGroupDiscussion.suggestedneeds_001_001 = obj.ContainsKey("suggestedneeds_001_001") ? obj["suggestedneeds_001_001"] : null;
                                            focusGroupDiscussion.road_001 = obj.ContainsKey("road_001") ? obj["road_001"] : null;
                                            focusGroupDiscussion._id = obj.ContainsKey("_id") ? Convert.ToInt32(obj["_id"]) : 0;
                                            focusGroupDiscussion._uuid = obj.ContainsKey("_uuid") ? obj["_uuid"] : null;
                                            focusGroupDiscussion._submission_time = obj.ContainsKey("_submission_time") ? Convert.ToDateTime(obj["_submission_time"]) : default(DateTime);
                                            focusGroupDiscussion._status = obj.ContainsKey("_status") ? obj["_status"] : null;
                                            focusGroupDiscussion._submitted_by = obj.ContainsKey("_submitted_by") ? obj["_submitted_by"] : null;

                                            var exists = context.FocusGroupDiscussions.FirstOrDefault(g => g._id == focusGroupDiscussion._id);

                                            if (exists == null)
                                            {
                                                context.FocusGroupDiscussions.Add(focusGroupDiscussion);
                                            }
                                            successful += 1;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogError($"Exception during deserialization or saving data: {ex.Message}");
                                        unsuccessful += 1;
                                        continue;
                                    }

                                }
                                context.SaveChanges();
                                _logger.LogInformation($"Successfulrecords:{successful}, Unsuccessfulrecords:{unsuccessful}: {response.StatusCode}");

                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"Exception during deserialization or saving data: {ex.Message}");
                        }
                    }
                    else
                    {
                        _logger.LogError($"Failed to fetch data from Kobo API. Status Code: {response.StatusCode}");
                    }

                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
            }
        }
    }
}
