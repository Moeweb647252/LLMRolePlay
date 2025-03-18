using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LLMRolePlay.Controllers
{
  public class DeleteTranslatorRequest
  {
    public required uint id { get; set; }
  }

  public partial class API : ControllerBase
  {
    [HttpPost("deleteTranslator")]
    [AllowAnonymous]
    public async Task<ApiResponse> DeleteTranslator([FromBody] DeleteTranslatorRequest data)
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();

      Translator? translator = await _dBContext.Translators.Where(t => t.Id == data.id).FirstOrDefaultAsync();
      if (translator == null) return ApiResponse.MessageOnly(500, "translator not found");
      if (translator.Model.Provider.UserId != user.Id) return ApiResponse.MessageOnly(500, "translator does not belong to user");

      await _dBContext.Translators.Where(t => t.Id == data.id).ExecuteDeleteAsync();
      await _dBContext.SaveChangesAsync();
      return ApiResponse.Success();
    }
  }
}