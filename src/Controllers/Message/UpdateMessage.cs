using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    public class UpdateMessageRequest
    {
      public required uint messageId { get; set; }
      public required string content { get; set; }
    }

    [HttpPost("updateMessage")]
    [AllowAnonymous]
    public async Task<ApiResponse> UpdateMessage([FromBody] UpdateMessageRequest data)
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();
      Message? message = await _dBContext.Messages.Include(m => m.Chat).Where(m => m.Id == data.messageId).FirstOrDefaultAsync();
      if (message == null) return ApiResponse.MessageOnly(404, "message not found");
      if (message.Chat.UserId != user.Id) return ApiResponse.MessageOnly(505, "message not belongs to current user");
      message.Content = data.content;
      await _dBContext.SaveChangesAsync();
      return ApiResponse.Success();
    }
  }
}