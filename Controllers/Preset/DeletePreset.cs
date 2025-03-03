using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    public class DeletePresetRequest
    {
      public required int presetId { get; set; }
    }
  }

  public partial class API : ControllerBase
  {
    [HttpPost("deletePreset")]
    [AllowAnonymous]
    public async Task<ApiResponse> DeletePreset([FromBody] DeletePresetRequest data)
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();

      Preset? preset = await _dBContext.Presets.Where((preset) => preset.Id == data.presetId).FirstOrDefaultAsync();
      if (preset == null) return ApiResponse.MessageOnly(404, "preset not found");
      if (preset.UserId != user.Id) return ApiResponse.MessageOnly(505, "preset not belongs to current user");
      _dBContext.Presets.Remove(preset);
      await _dBContext.SaveChangesAsync();
      return ApiResponse.Success();

    }
  }
}