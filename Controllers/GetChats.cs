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
      var chats = new List<object>();
      foreach (var chat in await _dBContext.Chats.Include(c => c.Participants).Where(c => c.UserId == user.Id).ToListAsync())
      {
        var participants = new List<object>();
        foreach (var _participant in chat.Participants)
        {
          var participant = await _dBContext.Participants
            .Include(p => p.Character)
            .Include(p => p.Template)
            .Include(p => p.Model)
            .Where(p => p.Id == _participant.Id)
            .FirstAsync();
          var presets = new List<object>();
          foreach (var presetId in participant.GetPresetIdList())
          {
            var preset = await _dBContext.Presets.Where(p => p.Id == presetId).FirstOrDefaultAsync();
            if (preset != null) presets.Add(new
            {
              id = preset.Id,
              name = preset.Name
            });
          }

          var tmpParticipant = new
          {
            id = participant.Id,
            name = participant.Name,
            character = new
            {
              id = participant.Character.Id,
              name = participant.Character.Name,
            },
            template = new
            {
              id = participant.Template.Id,
              name = participant.Template.Name,
            },
            model = new
            {
              id = participant.Model.Id,
              name = participant.Model.Name,
            },
            presets
          };
          participants.Add(tmpParticipant);
        }
        chats.Add(new
        {
          id = chat.Id,
          name = chat.Name,
          participants
        });
      }
      return ApiResponse.Success(new
      {
        chats
      });
    }
  }
}
