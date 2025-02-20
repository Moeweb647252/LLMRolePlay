using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{

  public class ChangeUserNameRequest
  {
    public required string username { get; set; }
  }

  public partial class API : ControllerBase
  {
    [HttpPost("changeUserName")]
    [AllowAnonymous]
    public async Task<ApiResponse> ChangeUserName(HttpContext req, [FromBody] ChangeUserNameRequest data)
    {
      User? user = await Models.User.GetUserByRequest(_dBContext, req);
      if (user == null) return ApiResponse.TokenError();
      user.UserName = data.username;
      user.MarkAsModified(_dBContext);
      await _dBContext.SaveChangesAsync();
      return ApiResponse.Success(null);
    }
  }
}
