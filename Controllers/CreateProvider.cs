using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    public class CreateProviderRequest
    {
      public required string name { get; set; }
      public required string type { get; set; }
      public required string settings { get; set; }
      public required string description { get; set; }
    }

    [HttpPost("createProvider")]
    [AllowAnonymous]
    public async Task<ApiResponse> CreateProvider(HttpContext req, [FromBody] CreateProviderRequest data)
    {
      User? user = await Models.User.GetUserByRequest(_dBContext, req);
      if (user == null) return ApiResponse.TokenError();
      Provider provider = await Provider.CreateProvider(_dBContext, data.name, data.type, data.settings, data.description);
      user.Providers.Add(provider);
      user.MarkAsModified(_dBContext);
      await _dBContext.SaveChangesAsync();
      return ApiResponse.Success(new
      {
        id = provider.Id
      });
    }
  }
}
