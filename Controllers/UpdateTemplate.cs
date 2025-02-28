using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LLMRolePlay.Controllers
{
  public class UpdateTemplateRequest
  {
    public required int templateId { get; set; }
    public string? name { get; set; } = null;
    public string? content { get; set; } = null;
    public string? description { get; set; } = null;
    public bool? isPublic { get; set; } = null;
  }

  public partial class API : ControllerBase
  {
    [HttpPost("updateTemplate")]
    [AllowAnonymous]
    public async Task<ApiResponse> UpdateTemplate([FromBody] UpdateTemplateRequest data)
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();

      Template? template = await _dBContext.Templates
        .Where(t => t.Id == data.templateId)
        .FirstOrDefaultAsync();

      if (template == null) return ApiResponse.MessageOnly(404, "template not found");
      if (template.UserId != user.Id) return ApiResponse.MessageOnly(403, "template does not belong to current user");

      if (data.name != null) template.Name = data.name;
      if (data.content != null) template.Content = data.content;
      if (data.description != null) template.Description = data.description;
      if (data.isPublic != null) template.IsPublic = data.isPublic.Value;

      _dBContext.Templates.Update(template);
      await _dBContext.SaveChangesAsync();

      return ApiResponse.Success();
    }
  }


}