using LLMRolePlay.Models;
using Microsoft.EntityFrameworkCore;

namespace LLMRolePlay.Apis
{
  public partial class Api
  {
    Models.DbContext dbContext;

    public Api(Models.DbContext dbContext)
    {
      this.dbContext = dbContext;
    }

    async Task<User> GetUser(HttpContext req)
    {
      var token = req.Request.Headers["Token"];
      var user = await dbContext.Users.SingleAsync(u => u.Token == token);
      return user;
    }
  }
}