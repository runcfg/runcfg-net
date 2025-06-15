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
    private readonly ClientConfig _clientConfig;
    private readonly HttpClient _httpClient;
    
    /// <summary>
    ///     Path to [project].runcfg file
    ///     Defaults to Current Working Direction + ./runcfg
    /// </summary>
    /// <param name="path">Path to [project].runcfg file</param>
    public Client(string path = "")
    {
        var file = File.ReadAllText(path != string.Empty ? path : Directory.GetCurrentDirectory() + "/.runcfg");
        _clientConfig = JsonSerializer.Deserialize<ClientConfig>(file);
        _httpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://runcfg.com")
        };
    }

    public async Task<T?> Load<T>()
    {
        _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"{_clientConfig.ClientToken}");        
        var request = _httpClient.Send(new HttpRequestMessage(HttpMethod.Get, $"/app/project/{_clientConfig.ProjectId}/view"));
        var content = (await request.Content.ReadAsStringAsync()).TrimStart('"').TrimEnd('"').Replace("\\", string.Empty);
        try
        {
            var instance = JsonSerializer.Deserialize<T>(content);
            return instance;
        } 
        catch (Exception ex) when (ex is ArgumentNullException or JsonException or NotSupportedException)
        {
            throw new Exception($"Failed to deserialize runcfg content for project: {_clientConfig.ProjectId}, reason: {ex.Message}");
        }
    }
}
