using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace LLMRolePlay.Controllers
{
  public class LoginRequest
  {
    public required string email { get; set; }
    public required string password { get; set; }
  }
  public partial class API : ControllerBase
  {
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ApiResponse> Login([FromBody] LoginRequest req)
    {
      User? user = await _dBContext.Users.Where((user) => user.Email == req.email & user.Password == req.password).FirstOrDefaultAsync();
      if (user == null) user = await _dBContext.Users.Where((user) => user.UserName == req.email & user.Password == req.password).FirstOrDefaultAsync();
      if (user != null)
      {
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