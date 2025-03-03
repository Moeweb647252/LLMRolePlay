using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    public class DeleteModelRequest
    {
      public required uint modelId { get; set; }
    }
  }

  public partial class API : ControllerBase
  {
    [HttpPost("deleteModel")]
    [AllowAnonymous]
    public async Task<ApiResponse> DeleteModel([FromBody] DeleteModelRequest data)
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();

      Model? model = await _dBContext.Models.Where((model) => model.Id == data.modelId).FirstOrDefaultAsync();
      if (model == null) return ApiResponse.MessageOnly(404, "Model not found");

      // Check if user has permission to delete this model
      // You may need to adjust this check based on your application's requirements
      Provider? provider = await _dBContext.Providers.FindAsync(model.ProviderId);
      if (provider == null || provider.UserId != user.Id)
        return ApiResponse.MessageOnly(403, "You don't have permission to delete this model");

      await model.Delete(_dBContext);
      return ApiResponse.Success();
    }
  }
}