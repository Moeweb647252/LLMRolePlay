using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    [HttpPost("getTranslators")]
    [AllowAnonymous]
    public async Task<ApiResponse> GetTranslators()
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();

      List<Translator> translators
        = await _dBContext.Translators.Where(t => t.UserId == user.Id).ToListAsync();
      return ApiResponse.Success(new
      {
        translators = translators.Select(t => new
        {
          id = t.Id,
          name = t.Name,
          description = t.Description,
          settings = t.Settings,
          modelId = t.ModelId,
          templateId = t.TemplateId,
          presetIds = t.PresetIds.Split(",").Select(uint.Parse).ToArray(),
          createdAt = t.CreatedAt
        })
      });
    }
  }
}