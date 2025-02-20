using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    [HttpPost("getProviders")]
    [AllowAnonymous]
    public async Task<ApiResponse> GetProviders(HttpContext req)
    {
      User? user = await Models.User.GetUserByRequest(_dBContext, req);
      if (user == null) return ApiResponse.TokenError();
      return ApiResponse.Success(new
      {
        providers = user.Providers
      });
    }
  }
}
