using System.Text.Json;
using System.Text.Json.Serialization;

namespace Runcfg;

[Serializable]
struct ClientConfig
{
    [JsonPropertyName("projectId")]
    public string ProjectId { get; set; }
    [JsonPropertyName("clientToken")]
    public string ClientToken { get; set; }
}

public sealed class Client
{
    ClientConfig ClientConfig { get; set; }
    HttpClient _httpClient;
    
    public Client()
    {
        var file = File.ReadAllText(Directory.GetCurrentDirectory() + "/.runcfg");
        ClientConfig = JsonSerializer.Deserialize<ClientConfig>(file);
        _httpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://runcfg.com")
        };
    }

    public async Task<T> Load<T>()
    {
        _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"{ClientConfig.ClientToken}");        
        var request = _httpClient.Send(new HttpRequestMessage(HttpMethod.Get, $"/app/project/{ClientConfig.ProjectId}/view"));
        var content = (await request.Content.ReadAsStringAsync()).TrimStart('"').TrimEnd('"').Replace("\\", string.Empty);
        var instance = JsonSerializer.Deserialize<T>(content);
        return instance;
    }
}