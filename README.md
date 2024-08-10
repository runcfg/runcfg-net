# Runcfg .NET Client

### Usage in projects

First download dependency using nuget
```shell
$  dotnet add package Runcfg --version 1.0.0
```

### Using your first config

1. Create an account at https://runcfg.com
2. Download your .runcfg file from your project page at https://runcfg.com by clicking (get .runcfg file)

![runcfg.PNG](runcfg.PNG)

3. Place your .runcfg file at the root of your project
4. Create an instance of the client in your code as follows:

```csharp
using System.Text.Json.Serialization;
using Runcfg;

[Serializable]
struct MyConfig
{
    [JsonPropertyName("version")]
    public string Version { get; init; }
    
    [JsonPropertyName("target")]
    public string Target { get; init; }
    
    [JsonPropertyName("enabled")]
    public string Enabled { get; init; }
} 

var client = new Client();
var config = await client.Load<MyConfig>();
Assert.Equal("1", config.Version);
```

Now your remote config is available in your specified type.