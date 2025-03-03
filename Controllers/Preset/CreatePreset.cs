using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public class CreatePresetRequest
  {
    public required string name { get; set; }
    public required string content { get; set; }
    public string? settings { get; set; }
    public required string description { get; set; }
    public bool isPublic { get; set; } = false;
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

      Preset preset = await Preset.CreatePreset(_dBContext, data.name, data.content, data.settings, data.description, user.Id, data.isPublic);
      await _dBContext.SaveChangesAsync();
      return ApiResponse.Success(new
      {
        id = preset.Id
      });
    }
  }
}
