using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    [HttpGet("getFile/{token}/{id}")]
    [AllowAnonymous]
    public async Task GetFile(string token, uint id)
    {
      if (token == null)
      {
        Response.StatusCode = 401;
        await Response.WriteAsync("Token not provided");
        return;
      }

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null)
      {
        Response.StatusCode = 401;
        await Response.WriteAsync("Invalid token");
        return;
      }

      Models.File? file = await _dBContext.Files.FirstOrDefaultAsync(file => file.Id == id && file.UserId == user.Id);
      if (file == null)
      {
        Response.StatusCode = 401;
        await Response.WriteAsync("File not found");
        return;
      }

      Response.ContentType = "application/octet-stream";
      await Response.Body.WriteAsync(file.Data);
      await Response.Body.FlushAsync();
    }
  }
}