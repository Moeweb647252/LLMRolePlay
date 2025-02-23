using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public class UpdateChatRequest
  {
    public required uint chatId { get; set; }
    public string? name { get; set; } = null;
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

      if (data.modelId != null)
      {
        Model? model = await Model.GetModelById(_dBContext, (uint)data.modelId);
        if (model == null) return ApiResponse.MessageOnly(500, "model not found");
        if (model.Provider.UserId != user.Id) return ApiResponse.MessageOnly(505, "model not belongs to current user");
        chat.ModelId = (uint)data.modelId;
      }

      if (data.characterId != null)
      {
        Character? character = await Character.GetCharacterById(_dBContext, (uint)data.characterId);
        if (character == null) return ApiResponse.MessageOnly(500, "character not found");
        if (character.UserId != user.Id) return ApiResponse.MessageOnly(505, "character not belongs to current user");
        chat.CharacterId = (uint)data.characterId;
      }

      if (data.presetId != null)
      {
        Preset? preset = await Preset.GetPresetById(_dBContext, (uint)data.presetId);
        if (preset == null) return ApiResponse.MessageOnly(500, "preset not found");
        if (preset.UserId != user.Id) return ApiResponse.MessageOnly(505, "preset not belongs to current user");
        chat.PresetId = (uint)data.presetId;
      }

      if (data.name != null) chat.Name = data.name;
      if (data.settings != null) chat.Settings = data.settings;

      chat.MarkAsModified(_dBContext);
      await _dBContext.SaveChangesAsync();
      return ApiResponse.Success();
    }
  }
}
