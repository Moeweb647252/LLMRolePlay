using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LLMRolePlay.Controllers
{
  public class GetFullChatRequest
  {
    public required uint chatId { get; set; }
  }
  public partial class API : ControllerBase
  {
    [HttpPost("getFullChat")]
    [AllowAnonymous]
    public async Task<ApiResponse> GetFullChat([FromBody] GetFullChatRequest data)
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();

      Chat? chat = await _dBContext.Chats
        .Include(c => c.Participants)
        .ThenInclude(p => p.Model)
        .Include(c => c.Participants)
        .ThenInclude(p => p.Character)
        .Include(c => c.Participants)
        .ThenInclude(p => p.Template)
        .Where(c => c.Id == data.chatId)
        .FirstOrDefaultAsync();
      if (chat == null) return ApiResponse.MessageOnly(404, "Chat not found");

      return ApiResponse.Success(new
      {
        chat = new
        {
          id = chat.Id,
          name = chat.Name,
          description = chat.Description,
          settings = chat.Settings,
          participants = await Utils.AwaitAll(chat.Participants.Select(async participant => new
          {
            id = participant.Id,
            name = participant.Name,
            model = participant.Model,
            settings = participant.Settings,
            presets = await participant.GetPresetList(_dBContext),
            character = participant.Character,
            template = participant.Template,
          }))
        }
      });
    }
  }
}
