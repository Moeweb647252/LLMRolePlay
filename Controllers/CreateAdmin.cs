using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    [HttpGet("createAdmin")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateAdmin(string token, string userName, string email, string password)
    {
      List<User> users = _dBContext.Users.Where((user) => user.Email == email).ToList();
      if (users.Count > 0)
      {
        return StatusCode(404, "email conflicted");
      }
      else
      {
        User user = await Models.User.CreateAdmin(_dBContext, userName, email, password);
        return StatusCode(200, user.Id);
      }
    }
  }
}
