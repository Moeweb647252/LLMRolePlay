using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    public class DeleteMessageRequest
    {
      public required ulong messageId { get; set; }
    }
  }

  public partial class API : ControllerBase
  {
    [HttpPost("deleteMessage")]
    [AllowAnonymous]
    public async Task<ApiResponse> DeleteMessage([FromBody] DeleteMessageRequest data)
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();

      Message? message = await _dBContext.Messages.Include(m => m.Chat).Where(m => m.Id == data.messageId).FirstOrDefaultAsync();
      if (message == null) return ApiResponse.MessageOnly(404, "Message not found");

      // Verify the user owns the chat containing this message
      if (message.Chat.UserId != user.Id) return ApiResponse.MessageOnly(403, "Message does not belong to current user");

      _dBContext.Messages.Remove(message);
      await _dBContext.SaveChangesAsync();
      return ApiResponse.Success();
    }
  }
}