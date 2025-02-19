using LLMRolePlay.Models;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    [HttpGet("login")]
    public async Task<IActionResult> Login(string email, string password)
    {
      List<User> users = _dBContext.Users.Where((user) => user.Email == email & user.Password == password).ToList();
      if (users.Count > 0)
      {
        User user = users[0];
        string token = await user.UpdateToken(_dBContext);
        return StatusCode(200, token);
      }
      else
      {
        return StatusCode(404);
      }
    }
  }
}