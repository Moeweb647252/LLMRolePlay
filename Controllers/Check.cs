using Microsoft.AspNetCore.Authorization;
using LLMRolePlay.Models;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    [HttpGet("check")]
    [AllowAnonymous]
    public async Task<ApiResponse> Check(HttpContext req)
    {
      User? user = await Models.User.GetUserByRequest(_dBContext, req);
      if (user == null) return ApiResponse.TokenError();
      return ApiResponse.Success(new
      {
        group = user.Group,
        id = user.Id,
        username = user.UserName,
        email = user.Email
      });
    }
  }
}