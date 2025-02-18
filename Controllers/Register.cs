using LLMRolePlay.Models;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    [HttpGet("register")]
    public async Task<ActionResult<Response>> Register(string userName,string email, string password)
    {
      using (var db = new DBContext())
      {
        List<User> users = db.Users.Where((user) => user.Email == email).ToList();
        if (users.Count > 0)
        {
          return new Response(404);
        }
        else
        {
          await Models.User.CreateUser(userName, email, password);
          return new Response(200);
        }
      }
    }
  }
}
