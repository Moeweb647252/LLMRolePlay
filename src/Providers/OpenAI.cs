using LLMRolePlay.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace LLMRolePlay.Providers.OpenAI
{
  public class Settings
  {
    public double? temperature;
    public ulong? max_tokens;
    public double? top_p;
  }

  public class StreamingOptions
  {
    public bool include_usage { get; set; }
  }

  public class CompletionRequest
  {
    public required string model { get; set; }
    public required IEnumerable<ChatMessage> messages { get; set; }
    public bool stream { get; } = true;
    public double? temperature { get; set; }
    public ulong? max_tokens { get; set; }
    public double? top_p { get; set; }
    public StreamingOptions? streaming_options { get; set; }
  }
  public class StreamingResponseChunkChoiceDelta
  {
    public string? content { get; set; }
    public string? reasoning { get; set; }
    public string? role { get; set; }
    public string? refusal { get; set; }
    public ulong? index { get; set; }
  }

  public class StreamingResponseChunkChoice
  {
    public required StreamingResponseChunkChoiceDelta delta { get; set; }
    public ulong? index { get; set; }
    public string? finish_reason { get; set; }
    public ulong? logprobs { get; set; }
  }

  public class StreamingResponseChunk
  {
    public string? id { get; set; }
    public StreamingResponseChunkChoice[]? choices { get; set; }
    public ulong? created { get; set; }
    public string? model { get; set; }
  }

  public class OpenAI
  {
    public Model Model { get; set; }
    public DBContext _dBContext { get; set; }

    public OpenAI(Model model, DBContext dBContext)
    {
      _dBContext = dBContext;
      Model = model;
    }

    public async IAsyncEnumerable<StreamingResponseChunk> Completion(List<ChatMessage> messages, string systemPrompt)
    {
      messages.Insert(0, new ChatMessage
      {
        content = systemPrompt,
        role = "system",
      });
      Console.WriteLine("Messages: " + JsonSerializer.Serialize(messages, new JsonSerializerOptions { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping }));
      var client = new HttpClient();
      client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Model.Provider.ApiKey}");
      Settings? settings = JsonSerializer.Deserialize<Settings>(Model.Settings);
      var content = new StringContent(JsonSerializer.Serialize(new CompletionRequest
      {
        model = Model.ModelName,
        messages = messages,
        temperature = settings?.temperature,
        max_tokens = settings?.max_tokens,
        top_p = settings?.top_p,
        streaming_options = new StreamingOptions { include_usage = true },
      }), Encoding.UTF8, "application/json");
      content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
      var req = new HttpRequestMessage(HttpMethod.Post, Model.Provider.BaseUrl + "/chat/completions");
      req.Content = content;
      var response = await client.SendAsync(req, HttpCompletionOption.ResponseHeadersRead);
      if (response.IsSuccessStatusCode)
      {
        var stream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(stream);
        while (!reader.EndOfStream)
        {
          string? line = await reader.ReadLineAsync();
          if (line != null)
          {
            Console.WriteLine(line);
            if (line == "data: [DONE]") break;
            if (line.StartsWith("data: "))
            {
              line = line[6..];
              var chunk = JsonSerializer.Deserialize<StreamingResponseChunk>(line);
              if (chunk != null)
              {
                yield return chunk;
              }
            }
          }
        }
      }
      else
      {
        Console.WriteLine(response.StatusCode);
        Console.WriteLine(await response.Content.ReadAsStringAsync());
      }
    }
  }
}