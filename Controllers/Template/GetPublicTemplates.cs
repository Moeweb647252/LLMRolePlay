using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    [HttpPost("getPublicTemplates")]
    [AllowAnonymous]
    public async Task<ApiResponse> GetPublicTemplates()
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();

      return ApiResponse.Success(new
      {
        templates = await _dBContext.Templates.Where(template => template.IsPublic).ToListAsync()
      });
    }
  }
}