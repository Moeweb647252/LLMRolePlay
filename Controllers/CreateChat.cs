using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    [HttpGet("createChat")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateChat(string token, string name, string settings, uint modelId, uint characterId, uint presetId)
    {
      User? user = await Models.User.GetUserByToken(_dBContext,token);
      if (user == null) return StatusCode(404, "token error");
      Chat? chat = await Chat.CreateChat(_dBContext, name, settings, modelId, characterId, presetId);
      if (chat == null) return StatusCode(404, "provided id can not cast to a specific object");
      user.Chats.Add(chat);
      user.MarkAsModified(_dBContext);
      await _dBContext.SaveChangesAsync();
      return StatusCode(200, chat.Id);
    }
  }
}
