using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    [HttpGet("getChat")]
    [AllowAnonymous]
    public async Task<ActionResult<GetChatResult>> GetChat(string token, uint chatId)
    {
      User? user = await Models.User.GetUserByToken(_dBContext, token);
      Chat? chat = await Chat.GetChatById(_dBContext, chatId);
      if (user == null || chat == null) return StatusCode(404);
      if (!user.Chats.Contains(chat)) return StatusCode(404, "chat not belongs to this user");
      return StatusCode(200, new GetChatResult(chat.Id, chat.Name, chat.Settings, chat.Model, chat.Character, chat.Preset, chat.Messages.Count));
    }
    public class GetChatResult(uint id, string name, string settings, Model model, Character character, Preset preset, int messageCount)
    {
      public uint Id { get; set; } = id;
      public string Name { get; set; } = name;
      public string Settings { get; set; } = settings;
      public Model Model { get; set; } = model;
      public Character Character { get; set; } = character;
      public Preset Preset { get; set; } = preset;
      public int MessageCount { get; set; } = messageCount;
    }
  }
}
