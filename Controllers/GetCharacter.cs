using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    [HttpGet("getCharacter")]
    [AllowAnonymous]
    public async Task<ActionResult<Character>> GetCharacter(string token, uint characterId)
    {
      User? user = await Models.User.GetUserByToken(_dBContext, token);
      Character? character = await Character.GetCharacterById(_dBContext, characterId);
      if (user == null || character == null) return StatusCode(404);
      if (!user.Characters.Contains(character)) return StatusCode(404, "character not belongs to this user");
      return StatusCode(200, character);
    }
  }
}
