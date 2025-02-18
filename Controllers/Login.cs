using LLMRolePlay.Models;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.APIs
{
  [ApiController]
  [Route("api")]
  public partial class API : ControllerBase
  {
    [HttpGet("login")]
    public async Task<ActionResult<Response>> Login(string email, string password)
    {
      using (var db = new DBContext())
      {
        List<User> users = db.Users.Where((user) => user.Email == email & user.Password == password).ToList();
        if (users.Count > 0)
        {
          User user = users[0];
          string token = await user.UpdateToken();
          return new Response(200, token);
        }
        else
        {
          return new Response(404);
        }
      }
    }
  }
}