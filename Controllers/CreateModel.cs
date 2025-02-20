using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    public class CreateModelRequest
    {
      public required uint providerId { get; set; }
      public required string name { get; set; }
      public required string settings { get; set; }
      public required string description { get; set; }
    }
    [HttpPost("createModel")]
    [AllowAnonymous]
    public async Task<ApiResponse> CreateModel(HttpContext req, [FromBody] CreateModelRequest data)
    {
      User? user = await Models.User.GetUserByRequest(_dBContext, req);
      Provider? provider = await Provider.GetProviderById(_dBContext, data.providerId);
      if (user == null || provider == null) return ApiResponse.TokenError();
      if (!user.Providers.Contains(provider)) return ApiResponse.Message(502, "provider not belongs to this user");

      Model model = await Model.CreateModel(_dBContext, data.name, data.settings, data.description);
      provider.Models.Add(model);
      provider.MarkAsModified(_dBContext);
      await _dBContext.SaveChangesAsync();
      return ApiResponse.Success(new
      {
        id = model.Id
      });
    }
  }
}
