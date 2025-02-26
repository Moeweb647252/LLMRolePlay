using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    public class DeleteParticipantRequest
    {
      public required uint participantId { get; set; }
    }
  }

  public partial class API : ControllerBase
  {
    [HttpPost("deleteParticipant")]
    [AllowAnonymous]
    public async Task<ApiResponse> DeleteParticipant([FromBody] DeleteParticipantRequest data)
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();

      Participant? participant = await _dBContext.Participants
        .Include(p => p.Chat)
        .Where(p => p.Id == data.participantId)
        .FirstOrDefaultAsync();

      if (participant == null) return ApiResponse.MessageOnly(404, "Participant not found");

      // Check if the user owns the chat that contains this participant
      if (participant.Chat.UserId != user.Id) return ApiResponse.MessageOnly(403, "Participant not belongs to current user's chat");

      _dBContext.Participants.Remove(participant);
      await _dBContext.SaveChangesAsync();
      return ApiResponse.Success();
    }
  }
}