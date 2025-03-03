using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{

  public partial class API : ControllerBase
  {
    [HttpPost("createFile")]
    [AllowAnonymous]
    public async Task<ApiResponse> CreateFile()
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();
      if (Request.ContentLength == null) return ApiResponse.MessageOnly(400, "no file uploaded");
      byte[] data = new byte[(long)Request.ContentLength];
      await Request.Body.ReadExactlyAsync(data.AsMemory(0, (int)Request.ContentLength));

      Models.File file = new Models.File(
        data: data,
        userId: user.Id
      );

      _dBContext.Files.Add(file);
      await _dBContext.SaveChangesAsync();

      return ApiResponse.Success(new
      {
        id = file.Id
      });
    }
  }
}