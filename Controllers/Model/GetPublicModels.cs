using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    [HttpPost("getPublicModels")]
    [AllowAnonymous]
    public async Task<ApiResponse> GetPublicModels()
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();

      return ApiResponse.Success(new
      {
        models = (await _dBContext.Models.Where(model => model.IsPublic).ToListAsync()).Select(model => new
        {
          name = model.Name,
          description = model.Description,
          isPublic = model.IsPublic,
          modelName = model.ModelName,
          id = model.Id
        })
      });
    }
  }
}
