using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    public class UpdateUserRequest
    {
      public string? username { get; set; } = null;
      public string? password { get; set; } = null;
      public string? email { get; set; } = null;
      public Group? group { get; set; } = null;
    }

    [HttpPost("updateUser")]
    [AllowAnonymous]
    public async Task<ApiResponse> UpdateUser([FromBody] UpdateUserRequest data)
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();

      if (data.username != null) user.UserName = data.username;
      if (data.password != null) user.Password = data.password;
      if (data.email != null) user.Email = data.email;
      if (data.group != null) user.Group = (Group)data.group;

      user.MarkAsModified(_dBContext);
      await _dBContext.SaveChangesAsync();
      return ApiResponse.Success(null);
    }
  }
}
