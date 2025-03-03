using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    [HttpPost("getTemplates")]
    [AllowAnonymous]
    public async Task<ApiResponse> GetTemplates()
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();
      var templates = _dBContext.Templates.Where(template => template.UserId == user.Id).ToList();
      return ApiResponse.Success(new
      {
        templates = templates
      });
    }
  }
}