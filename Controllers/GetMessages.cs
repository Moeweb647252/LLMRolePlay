using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    [HttpPost("getMessages")]
    [AllowAnonymous]
    public async Task<ApiResponse> GetMessages(uint chatId)
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();

      Chat? chat = await Chat.GetChatById(_dBContext, chatId);
      if (chat == null) return ApiResponse.TokenError();

      if (chat.UserId != user.Id) return ApiResponse.MessageOnly(500, "chat not belongs to this user");
      return ApiResponse.Success(chat.Messages);
    }
  }
}
