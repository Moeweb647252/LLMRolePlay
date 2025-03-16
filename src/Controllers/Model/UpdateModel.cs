using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    public class UpdateModelRequest
    {
      public required uint modelId { get; set; }
      public string? name { get; set; } = null;
      public string? settings { get; set; } = null;
      public string? description { get; set; } = null;
      public bool? isPublic { get; set; } = null;
    }
    [HttpPost("updateModel")]
    [AllowAnonymous]
    public async Task<ApiResponse> UpdateModel([FromBody] UpdateModelRequest data)
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();

      Model? model = await _dBContext.Models.Where(m => m.Id == data.modelId && m.Provider.UserId == user.Id).FirstOrDefaultAsync();
      if (model == null) return ApiResponse.MessageOnly(500, "model not found");

      if (data.name != null) model.Name = data.name;
      if (data.settings != null) model.Settings = data.settings;
      if (data.description != null) model.Description = data.description;
      if (data.isPublic != null) model.IsPublic = data.isPublic.Value;

      model.MarkAsModified(_dBContext);
      await _dBContext.SaveChangesAsync();
      return ApiResponse.Success();
    }
  }
}
