using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    public class DeleteTemplateRequest
    {
      public required int templateId { get; set; }
    }
  }

  public partial class API : ControllerBase
  {
    [HttpPost("deleteTemplate")]
    [AllowAnonymous]
    public async Task<ApiResponse> DeleteTemplate([FromBody] DeleteTemplateRequest data)
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();

      Template? template = await _dBContext.Templates.Where((template) => template.Id == data.templateId).FirstOrDefaultAsync();
      if (template == null) return ApiResponse.MessageOnly(404, "template not found");
      if (template.UserId != user.Id) return ApiResponse.MessageOnly(505, "template does not belong to current user");
      _dBContext.Templates.Remove(template);
      await _dBContext.SaveChangesAsync();
      return ApiResponse.Success();
    }
  }
}