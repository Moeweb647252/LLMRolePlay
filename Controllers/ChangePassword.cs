using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    [HttpGet("changePassword")]
    [AllowAnonymous]
    public async Task<IActionResult> ChangePassword(string token, string password)
    {
      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return StatusCode(404);
      user.Password = password;
      user.MarkAsModified(_dBContext);
      await _dBContext.SaveChangesAsync();
      return StatusCode(200);
    }
  }
}
