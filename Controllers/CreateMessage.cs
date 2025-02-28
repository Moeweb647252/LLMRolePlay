// filepath: /home/misaka/Code/LLMRolePlay/Controllers/CreateMessage.cs
using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    public class CreateMessageRequest
    {
      public required uint chatId { get; set; }
      public required string content { get; set; }
      public required string role { get; set; }
    }

    [HttpPost("createMessage")]
    [AllowAnonymous]
    public async Task<ApiResponse> CreateMessage([FromBody] CreateMessageRequest data)
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();

      Chat? chat = await _dBContext.Chats.Where(c => c.Id == data.chatId).FirstOrDefaultAsync();
      if (chat == null) return ApiResponse.MessageOnly(404, "chat not found");

      if (chat.UserId != user.Id) return ApiResponse.MessageOnly(403, "chat not belongs to current user");

      Message message = await Message.CreateMessage(_dBContext, data.role, data.content, null, chat.Id, null);
      await _dBContext.SaveChangesAsync();

      return ApiResponse.Success(new
      {
        id = message.Id
      });
    }
  }
}