using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    [HttpGet("changeUserName")]
    [AllowAnonymous]
    public async Task<IActionResult> ChangeUserName(string token, string userName)
    {
      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return StatusCode(404);
      user.UserName = userName;
      user.MarkAsModified(_dBContext);
      await _dBContext.SaveChangesAsync();
      return StatusCode(200);
    }
  }
}
