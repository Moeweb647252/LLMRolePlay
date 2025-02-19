using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    [HttpGet("createProvider")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateProvider(string token, string name,string type, string settings, string description)
    {
      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return StatusCode(404);
      Provider provider = await Provider.CreateProvider(_dBContext, name, type, settings, description);
      user.Providers.Add(provider);
      user.MarkAsModified(_dBContext);
      await _dBContext.SaveChangesAsync();
      return StatusCode(200, provider.Id);
    }
  }
}
