using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public class CreateTemplateRequest
  {
    public required string name { get; set; }
    public required string content { get; set; }
    public string? description { get; set; } = null;
    public bool isPublic { get; set; } = false;
  }

  public partial class API : ControllerBase
  {
    [HttpPost("createTemplate")]
    [AllowAnonymous]
    public async Task<ApiResponse> CreateTemplate([FromBody] CreateTemplateRequest data)
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();

      Template template = new Template
      {
        Name = data.name,
        Content = data.content,
        Description = data.description,
        IsPublic = data.isPublic,
        UserId = user.Id
      };
      await _dBContext.Templates.AddAsync(template);
      await _dBContext.SaveChangesAsync();

      return ApiResponse.Success(new
      {
        id = template.Id
      });
    }
  }
}