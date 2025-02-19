using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    [HttpGet("createPreset")]
    [AllowAnonymous]
    public async Task<IActionResult> CreatePreset(string token, string name, string settings, string description)
    {
      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return StatusCode(404);
      Preset preset = await Preset.CreatePreset(_dBContext, name, settings, description);
      user.Presets.Add(preset);
      user.MarkAsModified(_dBContext);
      await _dBContext.SaveChangesAsync();
      return StatusCode(200, preset.Id);
    }
  }
}
