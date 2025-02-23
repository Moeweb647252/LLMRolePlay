using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public class UpdateChatRequest
  {
    public required uint chatId;
    public string? name = null;
    public string? settings = null;
    public required uint modelId;
    public required uint characterId;
    public required uint presetId;
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

      Model? model = await Model.GetModelById(_dBContext, data.modelId);
      if (model == null) return ApiResponse.MessageOnly(500, "model not found");

      Character? character = await Character.GetCharacterById(_dBContext, data.characterId);
      if (character == null) return ApiResponse.MessageOnly(500, "character not found");

      Preset? preset = await Preset.GetPresetById(_dBContext, data.presetId);
      if (preset == null) return ApiResponse.MessageOnly(500, "preset not found");

      if (preset.UserId != user.Id) return ApiResponse.MessageOnly(505, "chat not belongs to current user");

      if (data.name != null) chat.Name = data.name;
      if (data.settings != null) chat.Settings = data.settings;
      chat.Model = model;
      chat.Character = character;
      chat.Preset = preset;

      chat.MarkAsModified(_dBContext);
      await _dBContext.SaveChangesAsync();
      return ApiResponse.Success();
    }
  }
}
