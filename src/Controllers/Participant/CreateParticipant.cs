using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LLMRolePlay.Controllers
{
  public class CreateParticipantRequest
  {
    public required uint chatId { get; set; }
    public required uint modelId { get; set; }
    public required uint characterId { get; set; }
    public required uint[] presetIds { get; set; }
    public required uint templateId { get; set; }
    public required string name { get; set; }
    public required string settings { get; set; }
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

      Model? model = await _dBContext.Models.Include(m => m.Provider).Where(m => m.Id == data.modelId).FirstOrDefaultAsync();
      if (model == null) return ApiResponse.MessageOnly(500, "model not found");
      if (model.Provider.UserId != user.Id && !model.IsPublic) return ApiResponse.MessageOnly(500, "model does not belong to user");

      Character? character = await _dBContext.Characters.Where(c => c.Id == data.characterId).FirstOrDefaultAsync();
      if (character == null) return ApiResponse.MessageOnly(500, "character not found");
      if (character.UserId != user.Id && !character.IsPublic) return ApiResponse.MessageOnly(500, "character does not belong to user");

      Template? template = await _dBContext.Templates.Where(t => t.Id == data.templateId).FirstOrDefaultAsync();
      if (template == null) return ApiResponse.MessageOnly(500, "template not found");
      if (template.UserId != user.Id && !template.IsPublic) return ApiResponse.MessageOnly(500, "template does not belong to user");

      foreach (var presetId in data.presetIds)
      {
        Preset? preset = await _dBContext.Presets.Where(p => p.Id == presetId).FirstOrDefaultAsync();
        if (preset == null) return ApiResponse.MessageOnly(500, "preset not found");
        if (preset.UserId != user.Id && !preset.IsPublic) return ApiResponse.MessageOnly(500, "preset does not belong to user");
      }

      Chat? chat = await _dBContext.Chats.Where(c => c.Id == data.chatId).FirstOrDefaultAsync();
      if (chat == null) return ApiResponse.MessageOnly(500, "chat not found");
      if (chat.UserId != user.Id) return ApiResponse.MessageOnly(500, "chat does not belong to user");

      Participant participant = new Participant(model.Id, character.Id, data.presetIds, chat.Id, data.templateId, data.name, data.settings);
      _dBContext.Participants.Add(participant);
      await _dBContext.SaveChangesAsync();

      return ApiResponse.Success(new
      {
        id = participant.Id
      });
    }
  }
}