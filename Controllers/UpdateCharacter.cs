using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public class UpdateCharacterRequest
  {
    public required uint characterId;
    public string? name = null;
    public string? settings = null;
    public string? description = null;
    public bool? isPublic = null;
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

      Character? character = await Character.GetCharacterById(_dBContext, data.characterId);
      if (character == null) return ApiResponse.MessageOnly(500, "character not found");

      if (character.UserId != user.Id) return ApiResponse.MessageOnly(505, "character not belongs to current user");

      if (data.name != null) character.Name = data.name;
      if (data.settings != null) character.Settings = data.settings;
      if (data.description != null) character.Description = data.description;
      if (data.isPublic != null) character.IsPublic = data.isPublic.Value;

      character.MarkAsModified(_dBContext);
      await _dBContext.SaveChangesAsync();
      return ApiResponse.Success();
    }
  }
}
