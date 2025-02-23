using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    public class DeleteProviderRequest
    {
      public required uint providerId { get; set; }
    }

    [HttpPost("deleteProvider")]
    [AllowAnonymous]
    public async Task<ApiResponse> DeleteProvider([FromBody] DeleteProviderRequest data)
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();
      Provider? provider = await _dBContext.Providers.Include(p => p.Models).Where(p => p.Id == data.providerId).FirstOrDefaultAsync();
      if (provider == null) return ApiResponse.MessageOnly(500, "provider not found");

      if (provider.UserId != user.Id) return ApiResponse.MessageOnly(505, "provider not belongs to current user");
      foreach (var model in provider.Models.ToList())
      {
        _dBContext.Models.Remove(model);
        await _dBContext.SaveChangesAsync();
      }
      _dBContext.Providers.Remove(provider);
      await _dBContext.SaveChangesAsync();
      return ApiResponse.Success();
    }
  }
}
