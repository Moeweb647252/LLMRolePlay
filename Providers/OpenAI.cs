using LLMRolePlay.Models;
using Newtonsoft.Json;
using System.Collections;
using System.Net.Http.Headers;
using System.Text.Json;

namespace LLMRolePlay.Providers
{
  public class OpenAISettings
  {
    public double? temperature;
    public ulong? max_tokens;
    public double? top_p;
  }

  public class OpenAICompletionRequest
  {
    public required string model { get; set; }
    public required IEnumerable<ChatMessage> messages { get; set; }
    public bool stream { get; } = true;
    public double? temperature { get; set; }
    public ulong? max_tokens { get; set; }
    public double? top_p { get; set; }
  }
  public class OpenAIStreamingResponseChunkChoiceDelta
  {
    public string? content { get; set; }
    public string? role { get; set; }
    public string? refusal { get; set; }
    public ulong? index { get; set; }
  }

  public class OpenAIStreamingResponseChunkChoice
  {
    public required OpenAIStreamingResponseChunkChoiceDelta delta { get; set; }
    public required string id { get; set; }
    public required ulong created { get; set; }
    public required string model { get; set; }
  }

  public class OpenAIStreamingResponseChunk
  {
    public string? id { get; set; }
    public OpenAIStreamingResponseChunkChoice[]? choices { get; set; }
    public string? created { get; set; }
    public string? model { get; set; }
  }

  public class OpenAI
  {
    public Participant Participant { get; set; }
    public DBContext _dBContext { get; set; }

    public OpenAI(Participant participant, DBContext dBContext)
    {
      _dBContext = dBContext;
      Participant = participant;
    }

    public async IAsyncEnumerable<OpenAIStreamingResponseChunk> Compeletion(IEnumerable<ChatMessage> _messages)
    {
      var client = new HttpClient();
      client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Participant.Model.Provider.ApiKey}");
      var request = new HttpRequestMessage(HttpMethod.Post, Participant.Model.Provider.BaseUrl + "/v1/chat/completions");
      OpenAISettings? settings = JsonConvert.DeserializeObject<OpenAISettings>(Participant.Model.Settings);
      List<ChatMessage> messages = ([
        new ChatMessage { Role = "system", Content = await Participant.MakeSystemPrompt(_dBContext) },
      ]);
      messages.Concat(_messages);
      request.Content = new StringContent(JsonConvert.SerializeObject(new OpenAICompletionRequest
      {
        model = Participant.Model.ModelName,
        messages = messages,
        temperature = settings?.temperature,
        max_tokens = settings?.max_tokens,
        top_p = settings?.top_p
      }));
      request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
      var response = await client.SendAsync(request);
      if (response.IsSuccessStatusCode)
      {
        var stream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(stream);
        while (!reader.EndOfStream)
        {
          string? line = await reader.ReadLineAsync();
          if (line != null)
          {
            var chunk = JsonConvert.DeserializeObject<OpenAIStreamingResponseChunk>(line);
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