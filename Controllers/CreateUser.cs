using LLMRolePlay.Models;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    [HttpGet("createUser")]
    public async Task<ActionResult<Response>> CreateUser(string token, string userName, string email, string password)
    {
      //鉴权
      if (!await Models.User.Authenticate(_dBContext, token, Group.Admin))
      {
        return new Response(404);
      }

      List<User> users = _dBContext.Users.Where((user) => user.Email == email).ToList();
      if (users.Count > 0)
      {
        return new Response(404);
      }
      else
      {
        await Models.User.CreateUser(_dBContext, userName, email, password);
        return new Response(200);
      }
    }
  }
}
