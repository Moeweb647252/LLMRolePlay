using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace LLMRolePlay
{
  public class TokenAuthentication : IAuthenticationHandler
  {
    private DBContext _dBContext;
    private HttpContext? _context;
    public TokenAuthentication(DBContext dBContext)
    {
      _dBContext = dBContext;
    }

    public async Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
    {
      _context = context;
    }
    // token 验证，添加权限
    public async Task<AuthenticateResult> AuthenticateAsync()
    {
      string? token = _context?.Request.Headers["Token"];
      if (token == null) return AuthenticateResult.Fail("null token");
      User? user = await User.GetUserByToken(_dBContext, token);
      if (user == null) return AuthenticateResult.Fail("user not found");

      ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
      foreach (Group group in Enum.GetValues<Group>())
      {
        if (user.Group.HasFlag(group))
        {
          claimsPrincipal.AddIdentity(new ClaimsIdentity(new List<Claim>
          {
            new Claim(ClaimTypes.Role, group.ToString())
          }));
        }
      }
      return AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, "TokenAuthentication"));
    }

    //验证失败
    public async Task ChallengeAsync(AuthenticationProperties? properties)
    {
      if (_context == null) return;
      _context.Response.StatusCode = 404;
      return;
    }

    //验证失败
    public async Task ForbidAsync(AuthenticationProperties? properties)
    {
      if (_context == null) return;
      _context.Response.StatusCode = 404;
      return;
    }


  }
}
