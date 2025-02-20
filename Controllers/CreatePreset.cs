using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public class CreatePresetRequest
  {
    public required string name { get; set; }
    public required string settings { get; set; }
    public required string description { get; set; }
  }

  public partial class API : ControllerBase
  {
    [HttpPost("createPreset")]
    [AllowAnonymous]
    public async Task<ApiResponse> CreatePreset(HttpContext req, [FromBody] CreatePresetRequest data)
    {
      User? user = await Models.User.GetUserByRequest(_dBContext, req);
      if (user == null) return ApiResponse.TokenError();
      Preset preset = await Preset.CreatePreset(_dBContext, data.name, data.settings, data.description);
      user.Presets.Add(preset);
      user.MarkAsModified(_dBContext);
      await _dBContext.SaveChangesAsync();
      return ApiResponse.Success(new
      {
        id = preset.Id
      });
    }
  }
}
