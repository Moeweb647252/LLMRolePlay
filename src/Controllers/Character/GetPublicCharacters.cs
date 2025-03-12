using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    [HttpPost("getPublicCharacters")]
    [AllowAnonymous]
    public async Task<ApiResponse> GetPublicCharacters()
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();

      return ApiResponse.Success(new
      {
        characters = await _dBContext.Characters.Where(character => character.IsPublic).ToListAsync()
      });
    }
  }
}