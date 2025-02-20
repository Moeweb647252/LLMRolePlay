using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    [HttpPost("getPresets")]
    [AllowAnonymous]
    public async Task<ApiResponse> GetPresets(HttpContext req)
    {
      User? user = await Models.User.GetUserByRequest(_dBContext, req);
      if (user == null) return ApiResponse.TokenError();
      return ApiResponse.Success(new
      {
        presets = user.Presets
      });
    }
  }
}
