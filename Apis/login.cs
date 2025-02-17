using Microsoft.EntityFrameworkCore;

namespace LLMRolePlay.Apis
{
  public partial class Api
  {
    public async Task<ApiResponse> Login(string email, string password)
    {
      var user = await dbContext.Users.Where(u => u.Email == email && u.Password == password).ToListAsync();
      if (user.Count != 0)
      {
        user[0].Token = Guid.NewGuid().ToString();
        await dbContext.SaveChangesAsync();
        return new ApiResponse
        {
          code = 200,
          message = "Login successful",
          data = user
        };
      }
      else
      {
        return new ApiResponse
        {
          code = 401,
          message = "Invalid email or password",
          data = null
        };
      }
    }
  }
}