using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    [HttpPost("getProviders")]
    [AllowAnonymous]
    public async Task<ApiResponse> GetProviders()
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();
      var providers = _dBContext.Providers.Where(provider => provider.UserId == user.Id).ToList();
      return ApiResponse.Success(new
      {
        providers = providers
      });
    }
  }
}
