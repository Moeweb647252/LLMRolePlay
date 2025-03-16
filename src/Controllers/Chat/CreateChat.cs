using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public class CreateChatRequest
  {
    public required string name { get; set; }
    public string? description { get; set; }
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

      Chat chat = new Chat
      {
        Name = data.name,
        Description = data.description,
        Settings = data.settings,
        UserId = user.Id
      };
      await _dBContext.Chats.AddAsync(chat);
      await _dBContext.SaveChangesAsync();
      return ApiResponse.Success(new
      {
        id = chat.Id
      });
    }
  }
}
