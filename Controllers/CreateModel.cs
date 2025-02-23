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
      public required string modelName { get; set; }
      public required string settings { get; set; }
      public required string description { get; set; }
    }
    [HttpPost("createModel")]
    [AllowAnonymous]
    public async Task<ApiResponse> CreateModel([FromBody] CreateModelRequest data)
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();

      Provider? provider = await Provider.GetProviderById(_dBContext, data.providerId);
      if (provider == null) return ApiResponse.TokenError();

      if (provider.UserId != user.Id) return ApiResponse.MessageOnly(502, "provider not belongs to this user");

      Model model = await Model.CreateModel(_dBContext, data.name, data.modelName, data.settings, data.description, provider.Id);
      await _dBContext.SaveChangesAsync();
      return ApiResponse.Success(new
      {
        id = model.Id
      });
    }
  }
}
