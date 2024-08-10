using System.Text.Json.Serialization;

namespace Runcfg.Test;

public class ClientTests
{

    [Serializable]
    struct TestConfig
    {
        [JsonPropertyName("version")]
        public string Version { get; init; }
        
        [JsonPropertyName("target")]
        public string Target { get; init; }
        
        [JsonPropertyName("enabled")]
        public string Enabled { get; init; }
    }    
    
    [Fact]
    public async void CreateClientTest()
    {
        var client = new Client();
        var config = await client.Load<TestConfig>();
        Assert.Equal("1", config.Version);
    }
}