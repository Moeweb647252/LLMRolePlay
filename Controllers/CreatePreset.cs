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
    public async Task<ApiResponse> CreatePreset([FromBody] CreatePresetRequest data)
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();

      Preset preset = await Preset.CreatePreset(_dBContext, data.name, data.settings, data.description, user.Id);
      await _dBContext.SaveChangesAsync();
      return ApiResponse.Success(new
      {
        id = preset.Id
      });
    }
  }
}
