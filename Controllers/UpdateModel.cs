using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    public class UpdateModelRequest
    {
      public required uint modelId;
      public string? name = null;
      public string? settings = null;
      public string? description = null;
    }
    [HttpPost("updateModel")]
    [AllowAnonymous]
    public async Task<ApiResponse> UpdateModel([FromBody] UpdateModelRequest data)
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();

      Model? model = await Model.GetModelById(_dBContext, data.modelId);
      if(model==null) return ApiResponse.MessageOnly(500, "model not found");

      if (data.name != null) model.Name = data.name;
      if (data.settings != null) model.Settings = data.settings;
      if (data.description != null) model.Description = data.description;

      model.MarkAsModified(_dBContext);
      await _dBContext.SaveChangesAsync();
      return ApiResponse.Success();
    }
  }
}
