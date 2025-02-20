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
  }
  public partial class API : ControllerBase
  {
    [HttpPost("createCharacter")]
    [AllowAnonymous]
    public async Task<ApiResponse> CreateCharacter(HttpContext req, [FromBody] CreateCharacterRequest data)
    {
      User? user = await Models.User.GetUserByRequest(_dBContext, req);
      if (user == null) return ApiResponse.TokenError();
      Character character = await Character.CreateCharacter(_dBContext, data.name, data.settings, data.description);
      user.Characters.Add(character);
      user.MarkAsModified(_dBContext);
      await _dBContext.SaveChangesAsync();
      return ApiResponse.Success(new
      {
        id = character.Id
      });
    }
  }
}
