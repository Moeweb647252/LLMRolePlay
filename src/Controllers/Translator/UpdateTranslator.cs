using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LLMRolePlay.Controllers
{
  public class UpdateTranslatorRequest
  {
    public required uint id { get; set; }
    public string? name { get; set; } = null;
    public string? description { get; set; } = null;
    public string? settings { get; set; } = null;
    public uint? templateId { get; set; } = null;
    public uint[]? presetIds { get; set; } = null;
    public uint? modelId { get; set; } = null;
  }

  public partial class API : ControllerBase
  {
    [HttpPost("updateTranslator")]
    [AllowAnonymous]
    public async Task<ApiResponse> UpdateTranslator([FromBody] UpdateTranslatorRequest data)
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();

      Translator? translator
        = await _dBContext.Translators.Where(t => t.Id == data.id).FirstOrDefaultAsync();
      if (translator == null) return ApiResponse.MessageOnly(500, "translator not found");
      if (translator.Model.Provider.UserId != user.Id) return ApiResponse.MessageOnly(500, "translator does not belong to user");
      if (data.name != null) translator.Name = data.name;
      if (data.description != null) translator.Description = data.description;
      if (data.settings != null) translator.Settings = data.settings;
      if (data.templateId != null)
      {
        Template? template = await _dBContext.Templates.Where(t => t.Id == data.templateId).FirstOrDefaultAsync();
        if (template == null) return ApiResponse.MessageOnly(500, "template not found");
        if (template.UserId != user.Id && !template.IsPublic) return ApiResponse.MessageOnly(500, "template does not belong to user");
        translator.TemplateId = data.templateId.Value;
      }
      if (data.presetIds != null)
      {
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
        translator.PresetIds = string.Join(",", data.presetIds);
      }
      if (data.modelId != null)
      {
        Model? model = await _dBContext.Models.Include(m => m.Provider).Where(m => m.Id == data.modelId).FirstOrDefaultAsync();
        if (model == null) return ApiResponse.MessageOnly(500, "model not found");
        if (model.Provider.UserId != user.Id && !model.IsPublic) return ApiResponse.MessageOnly(500, "model does not belong to user");
        translator.ModelId = data.modelId.Value;
      }
      translator.MarkAsModified(_dBContext);
      await _dBContext.SaveChangesAsync();
      return ApiResponse.Success();
    }
  }
}