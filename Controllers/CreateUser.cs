using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    public class CreateUserRequest
    {
      public required string username { get; set; }
      public required string email { get; set; }
      public required string password { get; set; }
      public required Group group { get; set; }
    }

    [HttpPost("createUser")]
    [Authorize(Roles = "Admin")]
    public async Task<ApiResponse> CreateUser([FromBody] CreateUserRequest data)
    {
      List<User> users = _dBContext.Users.Where((user) => user.Email == data.email).ToList();
      if (users.Count > 0)
      {
        return ApiResponse.MessageOnly(502, "email conflicted");
      }
      else
      {
        User user = await Models.User.CreateUser(_dBContext, data.username, data.email, data.password, data.group);
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
}
