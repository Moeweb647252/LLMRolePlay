using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    public class DeleteChatRequest
    {
      public required uint chatId { get; set; }
    }
  }

  public partial class API : ControllerBase
  {
    [HttpPost("deleteChat")]
    [AllowAnonymous]
    public async Task<ApiResponse> DeleteChat([FromBody] DeleteChatRequest data)
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();

      Chat? chat = await _dBContext.Chats.Where((chat) => chat.Id == data.chatId).FirstOrDefaultAsync();
      if (chat == null) return ApiResponse.MessageOnly(404, "Chat not found");
      if (chat.UserId != user.Id) return ApiResponse.MessageOnly(403, "Chat does not belong to current user");

      _dBContext.Chats.Remove(chat);
      await _dBContext.SaveChangesAsync();
      return ApiResponse.Success();
    }
  }
}