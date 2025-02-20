using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace LLMRolePlay.Controllers
{
  public class ChangePasswordRequest
  {
    public required string password { get; set; }
  }

  public partial class API : ControllerBase
  {
    [HttpPost("changePassword")]
    [AllowAnonymous]
    public async Task<ApiResponse> ChangePassword(HttpContext req, [FromBody] ChangePasswordRequest data)
    {
      User? user = await Models.User.GetUserByRequest(_dBContext, req);
      if (user == null) return ApiResponse.TokenError();
      user.Password = data.password;
      user.MarkAsModified(_dBContext);
      await _dBContext.SaveChangesAsync();
      return ApiResponse.Success(null);
    }
  }
}
