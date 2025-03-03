// filepath: /home/misaka/Code/LLMRolePlay/Controllers/GetPublicPresets.cs
using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    [HttpPost("getPublicPresets")]
    [AllowAnonymous]
    public async Task<ApiResponse> GetPublicPresets()
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();

      return ApiResponse.Success(new
      {
        presets = await _dBContext.Presets.Where(preset => preset.IsPublic).ToListAsync()
      });
    }
  }
}