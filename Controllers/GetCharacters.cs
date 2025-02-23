using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    [HttpPost("getCharacters")]
    [AllowAnonymous]
    public async Task<ApiResponse> GetCharacters()
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();
      var characters = _dBContext.Characters.Where(character => character.UserId == user.Id).ToList();
      return ApiResponse.Success(new
      {
        characters = characters
      });
    }
  }
}
