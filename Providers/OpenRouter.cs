using LLMRolePlay.Models;
using Newtonsoft.Json;
using System.Collections;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace LLMRolePlay.Providers
{
  public class OpenRouterSettings
  {
    public double? temperature;
    public ulong? max_tokens;
    public double? top_p;
  }

  public class OpenRouterCompletionRequestProvider
  {
    public string[]? order { get; set; }
    public bool? allow_feedbacks { get; set; }

  }
  public class OpenRouterCompletionRequest
  {
    public required string model { get; set; }
    public required IEnumerable<ChatMessage> messages { get; set; }
    public bool stream { get; } = true;
    public double? temperature { get; set; }
    public ulong? max_tokens { get; set; }
    public double? top_p { get; set; }
  }
  public class OpenRouterStreamingResponseChunkChoiceDelta
  {
    public string? content { get; set; }
    public string? role { get; set; }
    public string? refusal { get; set; }
    public ulong? index { get; set; }
  }

  public class OpenRouterStreamingResponseChunkChoice
  {
    public required OpenRouterStreamingResponseChunkChoiceDelta delta { get; set; }
    public required string id { get; set; }
    public required ulong created { get; set; }
    public required string model { get; set; }
  }

  public class OpenRouterStreamingResponseChunk
  {
    public string? id { get; set; }
    public OpenRouterStreamingResponseChunkChoice[]? choices { get; set; }
    public string? created { get; set; }
    public string? model { get; set; }
  }

  public class OpenRouter
  {
    public Participant Participant { get; set; }
    public DBContext _dBContext { get; set; }

    public OpenRouter(Participant participant, DBContext dBContext)
    {
      _dBContext = dBContext;
      Participant = participant;
    }

    public async IAsyncEnumerable<OpenRouterStreamingResponseChunk> Compeletion(List<ChatMessage> _messages)
    {
      var client = new HttpClient();
      client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Participant.Model.Provider.ApiKey}");
      OpenRouterSettings? settings = JsonConvert.DeserializeObject<OpenRouterSettings>(Participant.Model.Settings);
      List<ChatMessage> messages = ([
        new ChatMessage { role = "system", content = await Participant.MakeSystemPrompt(_dBContext) },
      ]);
      messages = messages.Concat(_messages.ToList()).ToList();
      var content = new StringContent(JsonConvert.SerializeObject(new OpenRouterCompletionRequest
      {
        model = Participant.Model.ModelName,
        messages = messages,
        temperature = settings?.temperature,
        max_tokens = settings?.max_tokens,
        top_p = settings?.top_p
      }), Encoding.UTF8, "application/json");
      content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
      var req = new HttpRequestMessage(HttpMethod.Post, Participant.Model.Provider.BaseUrl + "/chat/completions");
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
            if (line == "data: [DONE]") break;
            if (line.StartsWith("data: "))
            {
              line = line[6..];
              var chunk = JsonConvert.DeserializeObject<OpenRouterStreamingResponseChunk>(line);
              if (chunk != null)
              {
                yield return chunk;
              }
            }
          }
        }
      }
    }
  }
}