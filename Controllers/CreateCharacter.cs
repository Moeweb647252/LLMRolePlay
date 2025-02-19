using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    [HttpGet("createCharacter")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateCharacter(string token, string name, string settings, string description)
    {
      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return StatusCode(404);
      Character character = await Character.CreateCharacter(_dBContext, name, settings, description);
      user.Characters.Add(character);
      user.MarkAsModified(_dBContext);
      await _dBContext.SaveChangesAsync();
      return StatusCode(200, character.Id);
    }
  }
}
