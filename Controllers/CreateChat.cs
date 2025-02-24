using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public class CreateChatRequest
  {
    public required string name { get; set; }
    public required string settings { get; set; }
  }
  public partial class API : ControllerBase
  {
    [HttpPost("createChat")]
    [AllowAnonymous]
    public async Task<ApiResponse> CreateChat([FromBody] CreateChatRequest data)
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();

      Chat? chat = await Chat.CreateChat(_dBContext, data.name, data.settings, user.Id);
      if (chat == null) return ApiResponse.MessageOnly(502, "Chat creation failed");
      await _dBContext.SaveChangesAsync();
      return ApiResponse.Success(new
      {
        id = chat.Id
      });
    }
  }
}
