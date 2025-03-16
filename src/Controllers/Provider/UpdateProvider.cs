using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    public class UpdateProviderRequest
    {
      public required uint providerId { get; set; }
      public string? name { get; set; } = null;
      public string? type { get; set; } = null;
      public string? settings { get; set; } = null;
      public string? description { get; set; } = null;
    }

    [HttpPost("updateProvider")]
    [AllowAnonymous]
    public async Task<ApiResponse> UpdateProvider([FromBody] UpdateProviderRequest data)
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();


      Provider? provider = await _dBContext.Providers
        .Where(p => p.Id == data.providerId)
        .FirstOrDefaultAsync();
      if (provider == null) return ApiResponse.MessageOnly(500, "provider not found");

      if (provider.UserId != user.Id) return ApiResponse.MessageOnly(505, "provider not belongs to current user");

      if (data.name != null) provider.Name = data.name;
      if (data.type != null) provider.Type = data.type;
      if (data.settings != null) provider.Settings = data.settings;
      if (data.description != null) provider.Description = data.description;

      provider.MarkAsModified(_dBContext);
      await _dBContext.SaveChangesAsync();
      return ApiResponse.Success();
    }
  }
}
