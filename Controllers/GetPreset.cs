using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    [HttpGet("getPreset")]
    [AllowAnonymous]
    public async Task<ActionResult<Preset>> GetPreset(string token, uint presetId)
    {
      User? user = await Models.User.GetUserByToken(_dBContext, token);
      Preset? preset = await Preset.GetPresetById(_dBContext, presetId);
      if (user == null || preset == null) return StatusCode(404);
      if (!user.Presets.Contains(preset)) return StatusCode(404, "preset not belongs to this user");
      return StatusCode(200, preset);
    }
  }
}
