using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    [HttpGet("getProvider")]
    [AllowAnonymous]
    public async Task<ActionResult<Provider>> GetProvider(string token, uint providerId)
    {
      User? user = await Models.User.GetUserByToken(_dBContext, token);
      Provider? provider = await Provider.GetProviderById(_dBContext, providerId);
      if (user == null || provider == null) return StatusCode(404);
      if (!user.Providers.Contains(provider)) return StatusCode(404, "provider not belongs to this user");
      return StatusCode(200, provider);
    }
  }
}
