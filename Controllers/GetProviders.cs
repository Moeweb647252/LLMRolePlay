using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    [HttpPost("getProviders")]
    [AllowAnonymous]
    public async Task<ApiResponse> GetProviders()
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();
      var providers = _dBContext.Providers.Include(provider => provider.Models).Where(provider => provider.UserId == user.Id).ToList();
      return ApiResponse.Success(new
      {
        providers = providers.Select(provider => new
        {
          id = provider.Id,
          name = provider.Name,
          description = provider.Description,
          baseUrl = provider.BaseUrl,
          apiKey = provider.ApiKey,
          type = provider.Type,
          settings = provider.Settings,
          models = provider.Models.Select(model => new
          {
            id = model.Id,
            name = model.Name,
            modelName = model.ModelName,
            settings = model.Settings,
            description = model.Description,
            isPublic = model.IsPublic
          })
        })
      });
    }
  }
}
