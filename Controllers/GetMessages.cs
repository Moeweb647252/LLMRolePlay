using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    public class GetMessagesRequest
    {
      public required uint chatId { get; set; }
    }

    [HttpPost("getMessages")]
    [AllowAnonymous]
    public async Task<ApiResponse> GetMessages([FromBody] GetMessagesRequest data)
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();

      Chat? chat = await _dBContext.Chats.Include(c => c.Messages).Where(c => c.Id == data.chatId).FirstOrDefaultAsync();
      if (chat == null) return ApiResponse.TokenError();

      if (chat.UserId != user.Id) return ApiResponse.MessageOnly(500, "chat not belongs to this user");
      return ApiResponse.Success(
        new
        {
          messages = chat.Messages.Select(m => new
          {
            id = m.Id,
            content = m.Content,
            role = m.Role,
            createdAt = m.CreatedAt,
            participantId = m.ParticipantId,
          })
        }
      );
    }
  }
}
