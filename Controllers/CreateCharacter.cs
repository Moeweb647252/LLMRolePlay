using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public class CreateCharacterRequest
  {
    public required string name { get; set; }
    public required string settings { get; set; }
    public required string description { get; set; }
    public bool isPublic { get; set; } = false;
  }
  public partial class API : ControllerBase
  {
    [HttpPost("createCharacter")]
    [AllowAnonymous]
    public async Task<ApiResponse> CreateCharacter([FromBody] CreateCharacterRequest data)
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();

      Character character = await Character.CreateCharacter(_dBContext, data.name, data.settings, data.description, user.Id, data.isPublic);
      user.MarkAsModified(_dBContext);
      await _dBContext.SaveChangesAsync();
      return ApiResponse.Success(new
      {
        id = character.Id
      });
    }
  }
}
