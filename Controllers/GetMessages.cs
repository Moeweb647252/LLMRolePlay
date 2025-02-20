using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    [HttpPost("getMessages")]
    [AllowAnonymous]
    public async Task<ActionResult<List<Message>>> GetMessages(HttpContext req, uint chatId)
    {
      User? user = await Models.User.GetUserByRequest(_dBContext, req);
      Chat? chat = await Chat.GetChatById(_dBContext, chatId);
      if (user == null || chat == null) return StatusCode(404);
      if (!user.Chats.Contains(chat)) return StatusCode(404, "chat not belongs to this user");
      return StatusCode(200, chat.Messages);
    }
  }
}
