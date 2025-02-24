using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    [HttpPost("getChats")]
    [AllowAnonymous]
    public async Task<ApiResponse> GetChats()
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();

      return ApiResponse.Success(new
      {
        chats = (await _dBContext.Chats.Include(c => c.Participants).Where(c => c.UserId == user.Id).ToListAsync()).Select(chat => new
        {
          id = chat.Id,
          name = chat.Name,
          settings = chat.Settings,
          participants = chat.Participants.Select(participant => new
          {
            id = participant.Id,
            model = participant.Model,
            preset = participant.Preset,
            character = participant.Character
          })
        })
      });
    }
  }
}
