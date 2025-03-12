using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    public class DeleteCharacterRequest
    {
      public required uint characterId { get; set; }
    }
  }

  public partial class API : ControllerBase
  {
    [HttpPost("deleteCharacter")]
    [AllowAnonymous]
    public async Task<ApiResponse> DeleteCharacter([FromBody] DeleteCharacterRequest data)
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();

      Character? character = await Character.GetCharacterById(_dBContext, data.characterId);
      if (character == null) return ApiResponse.MessageOnly(404, "character not found");
      if (character.UserId != user.Id) return ApiResponse.MessageOnly(403, "character does not belong to current user");
      Models.File? file = await _dBContext.Files.Where(f => f.Id == character.Avatar).FirstOrDefaultAsync();
      if (file != null)
      {
        _dBContext.Files.Remove(file);
        await _dBContext.SaveChangesAsync();
      }
      await character.Delete(_dBContext);
      return ApiResponse.Success();
    }
  }
}