using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    public class UpdateParticipantRequest
    {
      public required uint participantId { get; set; }
      public uint? modelId { get; set; } = null;
      public uint? characterId { get; set; } = null;
      public uint[]? presetIds { get; set; } = null;
      public uint? templateId { get; set; } = null;
      public string? name { get; set; } = null;
      public string? settings { get; set; } = null;
    }

    [HttpPost("updateParticipant")]
    [AllowAnonymous]
    public async Task<ApiResponse> UpdateParticipant([FromBody] UpdateParticipantRequest data)
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();

      Participant? participant = await _dBContext.Participants
          .FindAsync(data.participantId);
      if (participant == null) return ApiResponse.MessageOnly(500, "participant not found");

      // Verify that the participant belongs to a chat owned by the user
      Chat? chat = await _dBContext.Chats.FindAsync(participant.ChatId);
      if (chat == null || chat.UserId != user.Id)
        return ApiResponse.MessageOnly(505, "participant not belongs to current user's chat");

      if (data.modelId != null) participant.ModelId = data.modelId.Value;
      if (data.characterId != null) participant.CharacterId = data.characterId.Value;
      if (data.presetIds != null) participant.PresetIds = string.Join(',', data.presetIds);
      if (data.templateId != null) participant.TemplateId = data.templateId.Value;
      if (data.name != null) participant.Name = data.name;
      if (data.settings != null) participant.Settings = data.settings;

      participant.MarkAsModified(_dBContext);
      await _dBContext.SaveChangesAsync();
      return ApiResponse.Success();
    }
  }
}