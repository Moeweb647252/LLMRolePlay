using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
      public string? description { get; set; }
      public required bool isPublic { get; set; }
    }
    [HttpPost("createModel")]
    [AllowAnonymous]
    public async Task<ApiResponse> CreateModel([FromBody] CreateModelRequest data)
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();

      Provider? provider = await _dBContext.Providers
        .Where(p => p.Id == data.providerId)
        .FirstOrDefaultAsync();
      if (provider == null) return ApiResponse.TokenError();

      if (provider.UserId != user.Id) return ApiResponse.MessageOnly(502, "provider not belongs to this user");

      Model model = new Model
      {
        ProviderId = data.providerId,
        Name = data.name,
        ModelName = data.modelName,
        Settings = data.settings,
        Description = data.description,
        IsPublic = data.isPublic
      };
      await _dBContext.Models.AddAsync(model);
      await _dBContext.SaveChangesAsync();
      return ApiResponse.Success(new
      {
        id = model.Id
      });
    }
  }
}
