using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace LLMRolePlay.Controllers
{
  public class LoginRequst
  {
    public required string email { get; set; }
    public required string password { get; set; }
  }
  public partial class API : ControllerBase
  {
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ApiResponse> Login([FromBody] LoginRequst req)
    {
      List<User> users = _dBContext.Users.Where((user) => user.Email == req.email & user.Password == req.password).ToList();
      if (users.Count > 0)
      {
        User user = users[0];
        string token = await user.UpdateToken(_dBContext);
        return ApiResponse.Success(new
        {
          token = token,
          group = user.Group,
          id = user.Id,
          username = user.UserName,
          email = user.Email
        });
      }
      else
      {
        return new ApiResponse(401, null, "email or password error");
      }
    }
  }
}