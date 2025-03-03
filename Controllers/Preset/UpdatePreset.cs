using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public class UpdatePresetRequest
  {
    public required uint presetId { get; set; }
    public string? name { get; set; } = null;
    public string? content { get; set; } = null;
    public string? settings { get; set; } = null;
    public string? description { get; set; } = null;
    public bool? isPublic { get; set; } = null;
  }

  public partial class API : ControllerBase
  {
    [HttpPost("updatePreset")]
    [AllowAnonymous]
    public async Task<ApiResponse> UpdatePreset([FromBody] UpdatePresetRequest data)
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();

      Preset? preset = await Preset.GetPresetById(_dBContext, data.presetId);
      if (preset == null) return ApiResponse.MessageOnly(500, "preset not found");

      if (preset.UserId != user.Id) return ApiResponse.MessageOnly(505, "preset not belongs to current user");

      if (data.name != null) preset.Name = data.name;
      if (data.content != null) preset.Content = data.content;
      if (data.settings != null) preset.Settings = data.settings;
      if (data.description != null) preset.Description = data.description;
      if (data.isPublic != null) preset.IsPublic = data.isPublic.Value;

      preset.MarkAsModified(_dBContext);
      await _dBContext.SaveChangesAsync();
      return ApiResponse.Success(new
      {
        id = preset.Id
      });
    }
  }
}
