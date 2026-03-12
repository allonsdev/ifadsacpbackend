using Newtonsoft.Json;
using sacpapi.Models;
using sacpapi.Data;
using Microsoft.Extensions.DependencyInjection; // Add this for creating scope

public class GroupsDataFetcher : BackgroundService
{
    private readonly ILogger<GroupsDataFetcher> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly string KoboApiBaseUrl = "https://kc.kobotoolbox.org/";
    private readonly string KoboApiEndpoint = "api/v1/data/1610185";
    private readonly string AccessToken = "b087792e3787c84913d1e62f7795bb5f133e5e59";

    public GroupsDataFetcher(ILogger<GroupsDataFetcher> logger, IHttpClientFactory httpClientFactory, IServiceScopeFactory serviceScopeFactory)
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

                                if (jsonString != null)
                                {
                                    List<APGroup> groupList = JsonConvert.DeserializeObject<List<APGroup>>(jsonString);

                                    if (groupList != null && groupList.Any())
                                    {
                                        foreach (var group in groupList)
                                        {

                                            var exists = context.APGroups.FirstOrDefault(g => g._id == group._id);

                                            if (exists == null)
                                            {
                                                context.APGroups.Add(group);
                                            }
                                        }

                                        int successful = await context.SaveChangesAsync();

                                        int unsuccessful = groupList.Count - successful;

                                        _logger.LogInformation($"Successful records: {successful}, Unsuccessful records: {unsuccessful}, Status code: {response.StatusCode}");
                                    }
                                }
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
