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
      public required string baseUrl { get; set; }
      public required string apiKey { get; set; }
      public required string settings { get; set; }
      public required string description { get; set; }
    }

    [HttpPost("createProvider")]
    [AllowAnonymous]
    public async Task<ApiResponse> CreateProvider([FromBody] CreateProviderRequest data)
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();

      Provider provider = await Provider.CreateProvider(_dBContext, data.name, data.type, data.settings, data.baseUrl, data.apiKey, data.description);
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
