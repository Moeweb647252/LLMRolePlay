using LLMRolePlay.Models;
using LLMRolePlay.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LLMRolePlay.Controllers
{

  public class CompeletionRequest
  {
    public required uint participantId;
  }
  public partial class API : ControllerBase
  {
    [HttpPost("compeletion")]
    [AllowAnonymous]
    public async Task Compeletion([FromBody] CompeletionRequest data)
    {
      string? token = Request.Headers["token"];
      if (token == null) return;
      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return;
      Participant? participant = await _dBContext.Participants.FirstOrDefaultAsync(p => p.Id == data.participantId);
      if (participant == null) return;
      Chat? chat = await _dBContext.Chats.Include(c => c.Messages).FirstOrDefaultAsync(c => c.Id == participant.ChatId);
      if (chat == null) return;
      if (participant.Model.Provider.Type == "openai")
      {
        var openai = new OpenAI(participant, _dBContext);
        var content = "";
        var stream = openai.Compeletion(chat.Messages.Select(m => new ChatMessage
        {
          Content = m.Content,
          Role = m.Role,
        })).GetAsyncEnumerator();
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
                await Response.WriteAsync("data: " + delta + "\n");
              }
            }
            else
            {
              break;
            }
          }
          else
          {
            await Response.WriteAsync(":LLMRolePlay Processing");
          }
        }
        var newMessage = new Message("assistant", content, 0, chat.Id, participant.Id);
        _dBContext.Messages.Add(newMessage);
        await _dBContext.SaveChangesAsync();
        await Response.WriteAsync("data: [DONE]");
      }
    }
  }
}