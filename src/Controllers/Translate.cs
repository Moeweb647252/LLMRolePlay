using LLMRolePlay.Models;
using LLMRolePlay.Providers.OpenAI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace LLMRolePlay.Controllers
{
  public class TranslateRequest
  {
    public required uint translatorId { get; set; }
    public required string targetLanguage { get; set; }
    public required string content { get; set; }
  }
  public partial class API : ControllerBase
  {
    [HttpPost("translate")]
    [AllowAnonymous]
    public async Task<object> Translate([FromBody] TranslateRequest data)
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();
      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();
      Translator? translator = await _dBContext.Translators.Include(t => t.Model).ThenInclude(m => m.Provider).Include(t => t.Template).FirstOrDefaultAsync(t => t.Id == data.translatorId);
      if (translator == null) return ApiResponse.MessageOnly(500, "translator not found");
      if (translator.UserId != user.Id) return ApiResponse.MessageOnly(500, "translator does not belong to user");
      Response.Headers.Append("Cache-Control", "no-cache");
      Response.Headers.Append("Connection", "keep-alive");
      if (translator.Model.Provider.Type == "openai")
      {
        var openai = new OpenAI(translator.Model, _dBContext);
        var content = "";
        var chunks = data.content.Chunk(50).Select(c => new String(c)).ToList();
        var stream = openai.Completion([
          new ChatMessage {
            content = JsonSerializer.Serialize(new {
              total_chunks = chunks.Count,
              chunks = chunks.Select((c, i)=> new {
                chunk_id = i+1,
                content = c
              }),
            }, new JsonSerializerOptions {
              Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            }),
            role = "user",
            name = "user",
          }
        ], await translator.MakeSystemPrompt(_dBContext, data.targetLanguage)).GetAsyncEnumerator();
        while (true)
        {
          var task = stream.MoveNextAsync().AsTask();
          if (await Task.WhenAny(Task.Delay(30000), task) == task)
          {
            if (task.Result)
            {
              var chunk = stream.Current;
              string? delta = chunk.choices?.FirstOrDefault()?.delta?.content;
              if (delta != null)
              {
                content += delta;
              }
            }
            else
            {
              break;
            }
          }
        }
        content = System.Text.RegularExpressions.Regex.Replace(content, @"<think>.*?</think>", "", System.Text.RegularExpressions.RegexOptions.Singleline);
        return ApiResponse.Success(new
        {
          content = content,
        });
      }
      else
      {
        return ApiResponse.MessageOnly(500, "translator provider not supported");
      }
    }
  }
}