using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public class CreateParticipantRequest
  {
    public required uint chatId { get; set; }
    public required uint modelId { get; set; }
    public required uint characterId { get; set; }
    public required uint presetId { get; set; }
    public required uint templateId { get; set; }
  }

  public partial class API : ControllerBase
  {
    [HttpPost("createParticipant")]
    [AllowAnonymous]
    public async Task<ApiResponse> CreateParticipant([FromBody] CreateParticipantRequest data)
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();

      Model? model = await Model.GetModelById(_dBContext, data.modelId);
      if (model == null) return ApiResponse.MessageOnly(500, "model not found");
      if (model.Provider.UserId != user.Id) return ApiResponse.MessageOnly(500, "model does not belong to user");

      Character? character = await Character.GetCharacterById(_dBContext, data.characterId);
      if (character == null) return ApiResponse.MessageOnly(500, "character not found");
      if (character.UserId != user.Id) return ApiResponse.MessageOnly(500, "character does not belong to user");

      Preset? preset = await Preset.GetPresetById(_dBContext, data.presetId);
      if (preset == null) return ApiResponse.MessageOnly(500, "preset not found");
      if (preset.UserId != user.Id) return ApiResponse.MessageOnly(500, "preset does not belong to user");

      Chat? chat = await Chat.GetChatById(_dBContext, data.chatId);
      if (chat == null) return ApiResponse.MessageOnly(500, "chat not found");
      if (chat.UserId != user.Id) return ApiResponse.MessageOnly(500, "chat does not belong to user");

      Participant participant = new Participant(data.modelId, data.characterId, data.presetId, chat.Id, data.templateId);
      _dBContext.Participants.Add(participant);
      await _dBContext.SaveChangesAsync();

      return ApiResponse.Success(new
      {
        id = participant.Id
      });
    }
  }
}