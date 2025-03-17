using LLMRolePlay.Models;
using LLMRolePlay.Providers.OpenAI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace LLMRolePlay.Controllers
{

  public class CompletionRequest
  {
    public required uint participantId { get; set; }
  }
  public partial class API : ControllerBase
  {
    [HttpGet("completion/{participantId}")]
    [AllowAnonymous]
    public async Task Completion(uint participantId)
    {
      string? token = Request.Headers["token"];
      if (token == null) return;
      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return;
      Participant? participant = await _dBContext.Participants.Include(p => p.Template).Include(p => p.Character).Include(p => p.Model).ThenInclude(m => m.Provider).FirstOrDefaultAsync(p => p.Id == participantId);
      if (participant == null) return;
      Chat? chat = await _dBContext.Chats.Include(c => c.Messages).FirstOrDefaultAsync(c => c.Id == participant.ChatId);
      ChatSettings? chatSettings = JsonSerializer.Deserialize<ChatSettings>(chat?.Settings ?? "{}");
      if (chat == null) return;
      Response.Headers.Append("Content-Type", "text/event-stream");
      Response.Headers.Append("Cache-Control", "no-cache");
      Response.Headers.Append("Connection", "keep-alive");
      if (participant.Model.Provider.Type == "openai")
      {
        var openai = new OpenAI(participant, _dBContext);
        var content = "";
        var stream = openai.Completion(await Utils.AwaitAll(chat.Messages.Select(async m =>
        {
          var participant = await _dBContext.Participants.Where(p => p.Id == m.ParticipantId).FirstOrDefaultAsync();
          return new ChatMessage
          {
            content = m.Content,
            role = participant?.Id == participantId ? "assistant" : "user",
            name = participant == null ? chatSettings?.NameOfUser : participant.Name,
          };
        }).ToList())).GetAsyncEnumerator();
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
                await Response.WriteAsync("data: " + JsonSerializer.Serialize(new
                {
                  delta
                }) + "\n\n");
              }
            }
            else
            {
              break;
            }
          }
          else
          {
            await Response.WriteAsync("data: {\"action\": \"keep-alive\", \"delta\": \"\"}\n\n");
          }
        }
        // Remove <think>...</think> tags from content  
        content = System.Text.RegularExpressions.Regex.Replace(content, @"<think>.*?</think>", "", System.Text.RegularExpressions.RegexOptions.Singleline);
        var newMessage = new Message
        {
          ChatId = chat.Id,
          ParticipantId = participantId,
          Content = content,
          CreatedAt = DateTime.UtcNow,
          Role = "assistant"
        };
        _dBContext.Messages.Add(newMessage);
        await _dBContext.SaveChangesAsync();
        await Response.WriteAsync("data: " + JsonSerializer.Serialize(new
        {
          id = newMessage.Id,
        }) + "\n\n");
        await Response.WriteAsync("data: [DONE]");
      }
    }
  }
}