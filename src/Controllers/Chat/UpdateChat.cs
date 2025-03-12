using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public class UpdateChatRequest
  {
    public required uint chatId { get; set; }
    public string? name { get; set; } = null;
    public string? description { get; set; } = null;
    public string? settings { get; set; } = null;
    public uint? modelId { get; set; } = null;
    public uint? characterId { get; set; } = null;
    public uint? presetId { get; set; } = null;
  }
  public partial class API : ControllerBase
  {
    [HttpPost("updateChat")]
    [AllowAnonymous]
    public async Task<ApiResponse> UpdateChat([FromBody] UpdateChatRequest data)
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      Chat? chat = await Chat.GetChatById(_dBContext, data.chatId);
      if (chat == null) return ApiResponse.MessageOnly(500, "chat not found");

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();

      if (data.name != null) chat.Name = data.name;
      if (data.description != null) chat.Description = data.description;
      if (data.settings != null) chat.Settings = data.settings;

      chat.MarkAsModified(_dBContext);
      await _dBContext.SaveChangesAsync();
      return ApiResponse.Success();
    }
  }
}
