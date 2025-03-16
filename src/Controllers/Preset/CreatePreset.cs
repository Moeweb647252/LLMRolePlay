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
    public string? description { get; set; }
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

      Preset preset = new Preset
      {
        Name = data.name,
        Content = data.content,
        Settings = data.settings,
        Description = data.description,
        IsPublic = data.isPublic,
        UserId = user.Id
      };
      await _dBContext.Presets.AddAsync(preset);
      await _dBContext.SaveChangesAsync();
      return ApiResponse.Success(new
      {
        id = preset.Id
      });
    }
  }
}
