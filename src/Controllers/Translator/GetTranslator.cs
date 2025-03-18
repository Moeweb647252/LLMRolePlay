using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LLMRolePlay.Controllers
{
  public class GetTranslatorRequest
  {
    public required uint id { get; set; }
  }

  public partial class API : ControllerBase
  {
    [HttpPost("getTranslator")]
    [AllowAnonymous]
    public async Task<ApiResponse> GetTranslator([FromBody] GetTranslatorRequest data)
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();

      Translator? translator
        = await _dBContext.Translators.Where(t => t.Id == data.id).FirstOrDefaultAsync();
      if (translator == null) return ApiResponse.MessageOnly(500, "translator not found");
      if (translator.Model.Provider.UserId != user.Id && !translator.Model.IsPublic) return ApiResponse.MessageOnly(500, "translator does not belong to user");
      return ApiResponse.Success(new
      {
        id = translator.Id,
        name = translator.Name,
        description = translator.Description,
        templateId = translator.TemplateId,
        presetIds = translator.PresetIds.Split(',').Select(uint.Parse).ToArray(),
        modelId = translator.ModelId,
        settings = translator.Settings
      });
    }
  }
}