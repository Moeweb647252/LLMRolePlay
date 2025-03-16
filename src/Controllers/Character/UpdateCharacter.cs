using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LLMRolePlay.Controllers
{
  public class UpdateCharacterRequest
  {
    public required uint characterId { get; set; }
    public string? name { get; set; } = null;
    public string? content { get; set; } = null;
    public string? settings { get; set; } = null;
    public string? description { get; set; } = null;
    public bool? isPublic { get; set; } = null;
  }
  public partial class API : ControllerBase
  {
    [HttpPost("updateCharacter")]
    [AllowAnonymous]
    public async Task<ApiResponse> UpdateCharacter([FromBody] UpdateCharacterRequest data)
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();

      Character? character = await _dBContext.Characters.Where(c => c.Id == data.characterId).FirstOrDefaultAsync();
      if (character == null) return ApiResponse.MessageOnly(500, "character not found");

      if (character.UserId != user.Id) return ApiResponse.MessageOnly(505, "character not belongs to current user");

      if (data.name != null) character.Name = data.name;
      if (data.content != null) character.Content = data.content;
      if (data.settings != null) character.Settings = data.settings;
      if (data.description != null) character.Description = data.description;
      if (data.isPublic != null) character.IsPublic = data.isPublic.Value;

      character.MarkAsModified(_dBContext);
      await _dBContext.SaveChangesAsync();
      return ApiResponse.Success();
    }
  }
}
