using PlatformService.DTOs;
using System.Text.Json;

namespace PlatformService.SyncDataServices.Http;

public class HttpCommandDataClient : ICommandDataClient
{
    private readonly HttpClient httpClient;
    private readonly IConfiguration config;

    public HttpCommandDataClient(HttpClient client, IConfiguration config)
    {
        httpClient = client;
        this.config = config;
    }

    public async Task SendPlatformToCommand(PlatformReadDto platform)
    {
        var httpContent = new StringContent(
            JsonSerializer.Serialize(platform),
            System.Text.Encoding.UTF8,
            "application/json");
        Console.WriteLine(config.GetSection("CommandServiceURL").Value);
        var response = await httpClient.PostAsync($"{config["CommandServiceURL"]}/Platforms", httpContent);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Ok");
        }
        else
        {
            Console.WriteLine("Not Ok");
        }        
    }
}
