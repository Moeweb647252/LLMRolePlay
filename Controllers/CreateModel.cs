using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    [HttpGet("createModel")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateModel(string token, uint providerId, string name, string settings, string description)
    {
      User? user = await Models.User.GetUserByToken(_dBContext, token);
      Provider? provider = await Provider.GetProviderById(_dBContext, providerId);
      if (user == null || provider == null) return StatusCode(404);
      if (!user.Providers.Contains(provider)) return StatusCode(404, "provider not belongs to this user");

      Model model = await Model.CreateModel(_dBContext, name, settings, description);
      provider.Models.Add(model);
      provider.MarkAsModified(_dBContext);
      await _dBContext.SaveChangesAsync();
      return StatusCode(200, model.Id);
    }
  }
}
