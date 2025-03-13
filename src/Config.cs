using System.Text.Json;

namespace LLMRolePlay.Config
{
  public class Config
  {
    public required string dbConnectionString { get; set; }
    public required string dbType { get; set; }
    public required string listen { get; set; }
    public bool useHttps { get; set; } = false;
    public string? certPath { get; set; }
    public string? keyPath { get; set; }

    public static Config Default()
    {
      return new Config
      {
        dbConnectionString = "Data Source=lrp.db",
        dbType = "sqlite",
        listen = "localhost:5000",
        useHttps = false,
        certPath = null,
        keyPath = null
      };
    }

    public static Config? Load(string path)
    {
      if (!File.Exists(path)) return null;
      return JsonSerializer.Deserialize<Config>(File.ReadAllText(path));
    }
  }
}