using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LLMRolePlay.Controllers
{
  public class CreateTranslatorRequest
  {
    public required string name { get; set; }
    public string? description { get; set; } = null;
    public required uint templateId { get; set; }
    public required uint[] presetIds { get; set; }
    public required uint modelId { get; set; }
    public required string settings { get; set; }
  }

  public partial class API : ControllerBase
  {
    [HttpPost("createTranslator")]
    [AllowAnonymous]
    public async Task<ApiResponse> CreateTranslator([FromBody] CreateTranslatorRequest data)
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();

      Template? template = await _dBContext.Templates.Where(t => t.Id == data.templateId).FirstOrDefaultAsync();
      if (template == null) return ApiResponse.MessageOnly(500, "template not found");
      if (template.UserId != user.Id && !template.IsPublic) return ApiResponse.MessageOnly(500, "template does not belong to user");

      Model? model = await _dBContext.Models.Include(m => m.Provider).Where(m => m.Id == data.modelId).FirstOrDefaultAsync();
      if (model == null) return ApiResponse.MessageOnly(500, "model not found");
      if (model.Provider.UserId != user.Id && !model.IsPublic) return ApiResponse.MessageOnly(500, "model does not belong to user");

      List<Preset?> presets = await Utils.AwaitAll(data.presetIds.Select(id => _dBContext.Presets.Where(p => p.Id == id).FirstOrDefaultAsync())
                      .ToList());
      if (presets.Any(p => p == null))
      {
        return ApiResponse.MessageOnly(500, "one or more presets not found");
      }
      if (presets.Any(p => p!.UserId != user.Id && !p.IsPublic))
      {
        return ApiResponse.MessageOnly(500, "one or more presets do not belong to user");
      }

      Translator translator = new Translator
      {
        ModelId = data.modelId,
        PresetIds = string.Join(",", data.presetIds),
        TemplateId = data.templateId,
        Name = data.name,
        Description = data.description,
        Settings = data.settings,
        UserId = user.Id
      }
      ;
      await _dBContext.Translators.AddAsync(translator);
      await _dBContext.SaveChangesAsync();
      return ApiResponse.Success(new
      {
        id = translator.Id
      });
    }
  }
}